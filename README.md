# ByteBox Backend

ByteBox is a cloud storage solution similar to Google Drive, designed to handle large-scale file uploads, storage, and management. This repository contains the backend implementation of ByteBox, which leverages AWS services for scalability, reliability, and performance.

## Features

1. **Multi-Part File Upload with AWS S3 Pre-Signed URLs**:
   - Files are uploaded directly from the client side to AWS S3 using pre-signed URLs.
   - This approach reduces the load on the server and allows handling a large number of concurrent file uploads efficiently.

2. **Asynchronous Thumbnail Generation with AWS Lambda**:
   - Thumbnails for uploaded files are generated asynchronously using AWS Lambda.
   - This ensures that the process is scalable and does not block the main application flow.

3. **Metadata Management**:
   - Metadata is maintained for each file to track its state, including the validity of pre-signed URLs.
   - This allows the system to invalidate pre-signed URLs when necessary, ensuring security and control over file access.

4. **Background Services**:
   - **Thumbnail Pre-Signed URL Refresh**: A background service periodically refreshes the pre-signed URLs for thumbnails to ensure they remain accessible.
   - **Permanent Deletion of Trashed Files**: Files moved to the trash are permanently deleted after a certain period, ensuring efficient storage management.

## Architecture Overview

- **AWS S3**: Used for storing files and generating pre-signed URLs for direct uploads.
- **AWS Lambda**: Handles asynchronous tasks such as thumbnail generation.
- **Background Services**: Implemented using Hangfire to handle periodic tasks like URL refresh and file deletion.
