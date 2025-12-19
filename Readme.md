# RAGSharp

RAGSharp é um framework **.NET** para construção de **chatbots corporativos baseados em RAG (Retrieval-Augmented Generation)**, com foco em **arquitetura limpa**, **extensibilidade** e **integração nativa com ASP.NET Core**.

O projeto foi desenvolvido como **demonstração arquitetural**, mostrando como estruturar um pipeline RAG desacoplado, testável e pronto para produção.

---

## ✨ Principais Features

- Pipeline RAG desacoplado e extensível
- Interfaces claras para:
  - Chunking
  - Embeddings
  - Vector Stores
  - LLM Providers
- Integração nativa com `Microsoft.Extensions.DependencyInjection`
- Implementações **mock** para testes e demonstração
- Pronto para uso com Swagger / Web API
- Foco em Clean Architecture e SOLID

---

## 🧱 Arquitetura

RAGSharp
├── Core
│ ├── Abstractions
│ │ ├── IChunker
│ │ ├── IEmbedder
│ │ ├── IVectorStore
│ │ ├── ILLMClient
│ │ └── IRagPipeline
│ ├── Models
│ ├── Pipelines
│ └── Extensions
│
├── Providers
│ └── Mock
│
├── Storage
│ └── InMemory
│
└── Api
└── Controllers
