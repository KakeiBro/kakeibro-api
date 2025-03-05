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
echo "Fetching images from repository..."
IMAGES=$(gcloud artifacts docker images list $REPOSITORY_NAME \
  --project=$PROJECT_ID \
  --location=$REGION \
  --format="value(DIGEST)" \
  --sort-by="~CREATE_TIME")

# Convert the list of digests into an array
IMAGE_ARRAY=($IMAGES)

# Check if there are more than 3 images
if [ ${#IMAGE_ARRAY[@]} -gt 3 ]; then
  echo "Found ${#IMAGE_ARRAY[@]} images. Keeping the 3 most recent ones and deleting the rest..."

  # Loop through the tags, skipping the first 3 (most recent)
  for ((i=3; i<${#IMAGE_ARRAY[@]}; i++)); do
    DIGEST=${IMAGE_ARRAY[$i]}
    echo "Deleting image: $DIGEST"
    gcloud artifacts docker images delete "$REPOSITORY_NAME@$DIGEST" \
      --project=$PROJECT_ID \
      --location=$REGION \
      --quiet \
      --force-delete-tags
  done
else
  echo "Only ${#TAGS_ARRAY[@]} tags found. No cleanup needed."
fi

echo "Cleanup complete!"