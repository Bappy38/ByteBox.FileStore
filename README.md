# ByteBox Backend

ByteBox is a cloud storage solution similar to Google Drive, designed to handle large-scale file uploads, storage, and management. This repository contains the backend implementation of ByteBox, which leverages AWS services for scalability, reliability, and performance. The backend is built using **CQRS (Command Query Responsibility Segregation)** with **MediatR** for clean separation of concerns and maintainability. It uses **SQL Server** as the primary database for storing metadata and will be deployed to **AWS ECS (Elastic Container Service)** in the future for scalable and containerized deployment.

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

5. **Message Broker with AWS SQS**:
   - AWS SQS is used as a message broker to handle asynchronous communication between services.
   - A custom wrapper library, **NexaWrap.SQS**, has been implemented and published as a NuGet package on [nuget.org](https://www.nuget.org) to simplify SQS integration and usage.

## Architecture Overview

- **CQRS with MediatR**: The backend follows the CQRS pattern, separating commands (write operations) and queries (read operations) for better scalability and maintainability. MediatR is used to handle commands, queries, and domain events.
- **SQL Server**: Used as the primary database to store file metadata, user information, and other application data.
- **AWS S3**: Used for storing files and generating pre-signed URLs for direct uploads.
- **AWS Lambda**: Handles asynchronous tasks such as thumbnail generation.
- **AWS SQS**: Acts as a message broker for asynchronous communication between services. The custom **NexaWrap.SQS** library simplifies integration and usage.
- **Background Services**: Implemented using **Hangfire** to handle periodic tasks like URL refresh and file deletion.
- **Future Deployment**: The application will be deployed to **AWS ECS (Elastic Container Service)** for containerized and scalable deployment.

--- 

For any questions or issues, please open an issue on the GitHub repository or contact the maintainers directly.

Happy coding! 🚀
