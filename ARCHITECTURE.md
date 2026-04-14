# Event-Driven Architecture Diagram

Rendered diagram image: [architecture-diagram.png](./docs/architecture-diagram.png)

```mermaid
flowchart LR
    U[Admin User]
    FE[React Admin Portal]
    API[Products Service API (.NET 8)]
    DB[(Products SQL Server DB)]
    BUS[(Message Broker / Event Bus)]
    ORD[Orders Service]
    PAY[Payments Service]
    NOTIF[Notification Service]

    U --> FE
    FE --> API
    API --> DB

    API -- ProductCreated --> BUS
    API -- ProductUpdated --> BUS

    BUS --> ORD
    BUS --> PAY
    BUS --> NOTIF

    ORD -- Reserve/Validate Product --> API
    PAY -- Payment Status Event --> BUS
```

## Notes
- `Products Service` owns product data and publishes product events.
- Other services consume events asynchronously for loose coupling.
- `Orders Service` can still call Products synchronously for real-time validation.
- This supports microservices growth while keeping Products independently deployable.
