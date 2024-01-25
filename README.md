# Currency rate monitor

## Local Environment Setup

### Prerequisites

- Docker

### Steps

1. Clone the repository:

    ```bash
    git clone https://github.com/your-username/project-name.git
    ```

2. Create docker volume for postgres:

    ```bash
    docker volume create pg_volume
    ```

3. Build docker compose:

    ```bash
    docker compose -f .\docker-compose.local.yml build
    ```

    Run docker compose from project root.

4. Run docker compose:

    ```bash
    docker compose -f .\docker-compose.local.yml up -d
    ```

    Run docker compose from project root

5. Access the application:

    Open your web browser and visit `http://localhost:5001/swagger/index.html`.

## Usage

Provide instructions on how to use the application or any additional setup required.