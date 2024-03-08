# Secret Vault

# 一、后端WebApi

## 1.目录

- `Application`

  > 应用层，不包含具体的业务逻辑

  - `Hwj.SecretValut.Application`

  > 应用层主项目，主要实现对数据进行转化、校验等，是前端和后端数据的"中转站"；
  >
  > 被接口调用，自己调用Domain层

  - `Hwj.SecretValut.Application.AutoMapper`

  > 应用层数据处理项目，使用AutoMapper对前端和后端的数据进行转化

  - `Hwj.SecretValut.Application.Validator`

  > 应用层数据校验项目，使用验证器对前端提供参数进行校验

- `Domain`

  > 领域层，包含核心业务逻辑，定义领域模型

  - `Hwj.SecretVault.Domain.Service`

  > 领域层Service项目，根据业务逻辑对传递来的要求进行实现
  >
  > 被应用层调用，自己调用仓储(Repository)

- `Infrastructure.Core`

  > 基础服务层-核心，主要的技术实现

  - `Hwj.SecretVault.Infra.Cache`

  > 基础服务层的缓存相关，负责对应数据Dto的缓存操作

  - `Hwj.SecretVault.Infra.Entity`

  > 基础服务层的实体，包括Dto、Enum、Params等
  >
  > Dto系列实体，是交由前端使用的数据结构

  - `Hwj.SecretVault.Infra.Repository`

  > 基础服务层的仓储，主要是实现与数据库的交互：增删查改
  >
  > App系列实体(此部分的特有实体，命名无所谓)，是交由数据库使用的数据结构

- `Infrastructure.Common`

  > 基础服务层-通用，包括通用工具类及扩展
  >
  > 不在项目中实现，通过引用aow_commons项目引入

  - `Hwj.Aow.Extensions`
    - `Hwj.Aow.Infra.Repository.Extension`
  - `Hwj.Aow.Utils`
    - `Hwj.Aow.Utils.CommonHelper`
    - `Hwj.Aow.Utils.RedisHelper`

- `Presentation`

  > 展现层，暴露给前端，表现为接口(Controller)

  - `Properties`

    - `launchSettings.json`

    > ASP.NET Core应用特有的配置标准，用于应用的启动准备工作

  - `Configurations`

  > 项目的配置，包括依赖注入和AutoMapper、Consul等的配置

  - `Controllers`

  > 控制器，接口API

  - `Filters`

  > 此处主要是Swagger文件上传相关过滤

  - `Handlers`

  > 此处主要是Jwt授权中对授权方案的处理
