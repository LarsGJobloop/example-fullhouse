#!/usr/bin/env bash
# Dead simple script to reconcile the compose deployment.

# Exit on error, unset variables, and pipefail.
set -euo pipefail

echo "Reconciliation started"

# Navigate to the repository
cd /opt/app

# Log the old revision
old_revision=$(git rev-parse HEAD)
echo "Old revision: $old_revision"

# Fetch the latest changes
echo "Fetching the latest changes"
git fetch origin

# Log the new revision
new_revision=$(git rev-parse HEAD)
echo "New revision: $new_revision"

# Reset the repository to the latest changes
echo "Overwriting local repository with content of remote repository"
git reset --hard "origin/${git_repository_branch}"

# Pull the latest images
echo "Pulling the latest images"
docker compose pull

# Start the services without building new images
echo "Redeploying services"
compose_full_path="/opt/app/${compose_file_path}"
docker compose --file "$compose_full_path" up --detach --no-build --remove-orphans

# Log the end of the reconciliation
echo "Reconciliation completed"
