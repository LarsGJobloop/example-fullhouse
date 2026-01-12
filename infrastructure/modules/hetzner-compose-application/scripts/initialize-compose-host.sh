#!/usr/bin/env bash
# Dead simple script to initialize the compose host.

# Exit on error, unset variables, and pipefail.
set -euo pipefail

# Set noninteractive mode
export DEBIAN_FRONTEND=noninteractive
export DEBCONF_NONINTERACTIVE_SEEN=true

# Install prerequisites
apt-get update -y

# Add Docker GPG key
install -m 0755 -d /etc/apt/keyrings
curl -fsSL https://download.docker.com/linux/debian/gpg -o /etc/apt/keyrings/docker.asc
chmod a+r /etc/apt/keyrings/docker.asc

# Add Docker repository
cat >/etc/apt/sources.list.d/docker.sources <<EOF
Types: deb
URIs: https://download.docker.com/linux/debian
Suites: $(. /etc/os-release && echo "$VERSION_CODENAME")
Components: stable
Signed-By: /etc/apt/keyrings/docker.asc
EOF

# Update package list
apt-get update -y

# Install Docker and Compose
apt-get install -y \
  docker-ce \
  docker-ce-cli \
  containerd.io \
  docker-buildx-plugin \
  docker-compose-plugin

# Verify installation
docker --version
docker compose version

# Clone repo (idempotent)
if [ ! -d /opt/app/.git ]; then
  git clone --branch "${git_repository_branch}" "${git_repository_url}" /opt/app
fi

# Mark initialization complete
touch /var/lib/compose-host.initialized
