# 内容清单
~~~
** DDD领域模型
** 集成AutoFac、Log4net、MediatR、Jwt认证、Redis、MemoryCache
** 引入ORM：EF、Dapper
** 集成RabbitMQ，MongoDB
** 集成Polly
** 集成单元测试：Xunit、Mock
** 加入VUE前端页面
** 集成FluentValidation，自定义验证功能
** 集成Redis哨兵（sentinel）模式、分片（cluster）模式
** 集成Redis主从模式
** 集成Jaeger分布式追踪组件
** 集成Consul分布式服务注册和发现组件
** 集成Ocelot分布式统一网关入口点
** 集成IdentityServer4 验证
** 集成Nginx+Ocelot+Consul
** 集成CI/CD环境，Jenkins+Harbor+Natapp
** 配置docker启动
** 配置Docker-compose.yml，批量启动所需的组件（cwy516project+consul+nginx+redis）(todo:mongodb)
** 集成ElasticSearch
** 集成Kafka消息队列
** 集成Elasticsearch+Logstash+Kibana(ELK日志系统)(Filebeat+Kafka)
** 集成Kafka集群
** 集成RabbitMq集群（3.8+3.9，区别就是erlang.cookie的配置方式，最新的硬盘空间占用少）
** 集成App.Metrics+InfluxDB+Grafana(todo)
** 集成Quartz定时任务调度（最新的3.3.3暂时无法安装）
** 集成事件驱动：不同服务相互调用，只需要定义一个接口，其他接口通过事件响应的方式调用(todo:目前集成了RabbitMq，后续考虑集成其他消息队列)
** 配置k8s集群
** 集成gRPC（todo）
** 集成apollo(阿波罗)配置中心（todo)
** 集成Mongodb集群（todo）
** 了解Socket通信（todo）
** 集成CAP分布式事务框架（todo）
** 集成云原生（todo)
** 熟悉并精通EF core(todo)
** 探索ServiceStack（todo)
** 了解MEF注入的方式(todo)
 
** 源码解读（Swagger，MediatR，MongoDB，StackExchange.Redis，JWT，IdentityServer4）
** 源码解读（aspnetcore 3.1 todo)
~~~


# 启动的准备工作
~~~
 1、配置mysql主从数据库（如果有的话），否则就只配置一个
 2、启动redis
 3、启动mongodb
 4、启动consul
 5、启动rabbitmq，如果有集群，通过docker-compose启动了容器之后，需要进入容器将节点加入集群（rabbitmqctl join_cluster --ram rabbit@rabbit1)
~~~
