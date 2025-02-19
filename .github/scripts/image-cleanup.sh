#!/bin/bash

# Variables
SERVICE_NAME=$SERVICE_NAME
REGION=$REGION
PROJECT_ID=$PROJECT_ID
REPOSITORY_NAME=$REPOSITORY_NAME
IMAGE_NAME=$IMAGE_NAME

# Authenticate to Google Cloud (already done in the workflow, but included for completeness)
echo "Authenticating to Google Cloud..."
gcloud auth activate-service-account --key-file=$GOOGLE_APPLICATION_CREDENTIALS

# Get the list of tags for the specified package, sorted by creation time in descending order
echo "Fetching tags for package..."
TAGS=$(gcloud artifacts tags list \
  --project=$PROJECT_ID \
  --location=$REGION \
  --repository=$REPOSITORY_NAME \
  --package=$IMAGE_NAME \
  --format="value(name)" \
  --sort-by=~create_time)

# Convert the list of tags into an array
TAGS_ARRAY=($TAGS)

# Check if there are more than 3 tags
if [ ${#TAGS_ARRAY[@]} -gt 3 ]; then
  echo "Found ${#TAGS_ARRAY[@]} tags. Keeping the 3 most recent ones and deleting the rest..."

  # Loop through the tags, skipping the first 3 (most recent)
  for ((i=3; i<${#TAGS_ARRAY[@]}; i++)); do
    TAG=${TAGS_ARRAY[$i]}
    echo "Deleting tag: $TAG"
    gcloud artifacts tags delete $TAG \
      --project=$PROJECT_ID \
      --location=$REGION \
      --repository=$REPOSITORY_NAME \
      --package=$IMAGE_NAME \
      --quiet
  done
else
  echo "Only ${#TAGS_ARRAY[@]} tags found. No cleanup needed."
fi

echo "Cleanup complete!"