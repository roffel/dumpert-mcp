#!/bin/bash

# Docker Hub publish script for DumpertMCP
# Usage: ./publish-docker.sh [version]

set -e

# Configuration
DOCKER_REPO="roffel"
IMAGE_NAME="dumpertmcp"
FULL_IMAGE_NAME="$DOCKER_REPO/$IMAGE_NAME"

# Get version from argument or use current timestamp
VERSION=${1:-$(date +%Y%m%d-%H%M%S)}
LATEST_TAG="latest"
VERSION_TAG="v$VERSION"

echo "🚀 Building and publishing DumpertMCP to Docker Hub"
echo "📦 Repository: $FULL_IMAGE_NAME"
echo "🏷️  Version: $VERSION_TAG"
echo ""

# Build the Docker image
echo "🔨 Building Docker image..."
docker build -t "$FULL_IMAGE_NAME:$VERSION_TAG" -t "$FULL_IMAGE_NAME:$LATEST_TAG" .

if [ $? -eq 0 ]; then
    echo "✅ Docker image built successfully!"
else
    echo "❌ Docker build failed!"
    exit 1
fi

# Ask for confirmation before pushing
echo ""
read -p "🤔 Do you want to push to Docker Hub? (y/N): " -n 1 -r
echo ""

if [[ $REPLY =~ ^[Yy]$ ]]; then
    echo "📤 Pushing to Docker Hub..."
    
    # Push both version and latest tags
    docker push "$FULL_IMAGE_NAME:$VERSION_TAG"
    docker push "$FULL_IMAGE_NAME:$LATEST_TAG"
    
    if [ $? -eq 0 ]; then
        echo ""
        echo "🎉 Successfully published to Docker Hub!"
        echo "📋 Image: $FULL_IMAGE_NAME:$VERSION_TAG"
        echo "📋 Latest: $FULL_IMAGE_NAME:$LATEST_TAG"
        echo ""
        echo "🐳 Pull with: docker pull $FULL_IMAGE_NAME:$VERSION_TAG"
        echo "🐳 Run with: docker run -p 5000:5000 $FULL_IMAGE_NAME:$VERSION_TAG"
    else
        echo "❌ Failed to push to Docker Hub!"
        exit 1
    fi
else
    echo "⏭️  Skipping push to Docker Hub"
    echo "🐳 You can manually push with:"
    echo "   docker push $FULL_IMAGE_NAME:$VERSION_TAG"
    echo "   docker push $FULL_IMAGE_NAME:$LATEST_TAG"
fi

