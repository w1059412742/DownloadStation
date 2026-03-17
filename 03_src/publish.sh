#!/bin/bash

# Softstation 一键发布与推送脚本 (Linux/macOS)

# ========== 配置区域 ==========
# 请在此处填写您的私有仓库地址
REGISTRY="dockerhub.heitang.top:8443"
IMAGE_NAME="softstation"

#拉代码
git reset --hard HEAD
git pull

ch *


# 1. 获取动态版本号
read -p "请输入发布版本号 (例如 1.0.1): " VERSION

if [ -z "$VERSION" ]; then
    echo -e "\033[33m⚠️ 版本号不能为空，脚本退出。\033[0m"
    exit 1
fi

# 完整的镜像全称
VERSION_TAG="$REGISTRY/$IMAGE_NAME:$VERSION"
LATEST_TAG="$REGISTRY/$IMAGE_NAME:latest"

# 2. 确保在脚本所在目录下运行
cd "$(dirname "$0")"

echo -e "\033[36m--- 🚀 开始构建 Docker 镜像: $VERSION_TAG ---\033[0m"

# 3. 执行 Docker 构建
docker build -t "$VERSION_TAG" -f Dockerfile.Synology .

if [ $? -ne 0 ]; then
    echo -e "\033[31m❌ 构建失败，请检查 Dockerfile 或网络连接。\033[0m"
    exit 1
fi

# 4. 打上 latest 标签
echo -e "\033[36m--- 🏷️ 正在通过 $VERSION 制作 latest 标签 ---\033[0m"
docker tag "$VERSION_TAG" "$LATEST_TAG"

echo -e "\033[32m--- ✅ 构建与打标成功，准备推送... ---\033[0m"

# 5. 执行 Docker 推送
echo -e "\033[36m--- ⬆️ 推送版本号标签: $VERSION_TAG ---\033[0m"
docker push "$VERSION_TAG"

echo -e "\033[36m--- ⬆️ 推送 latest 标签: $LATEST_TAG ---\033[0m"
docker push "$LATEST_TAG"

if [ $? -ne 0 ]; then
    echo -e "\033[31m❌ 推送失败，请确保已执行 docker login 登录到私有仓库。\033[0m"
    exit 1
fi

echo -e "\033[36m--- 🎉 一键发布完成！ ---\033[0m"
