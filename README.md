# 🏗️ معماری DDD و Clean Architecture

![dotnet-version](https://img.shields.io/badge/dotnet%20version-net10.0-blue)

<div dir="rtl">

این پروژه یک نمونه پیاده‌سازی **معماری تمیز (Clean Architecture)** به همراه اصول **Domain-Driven Design (DDD)** و الگوی **CQRS** با استفاده از **.NET 10** می‌باشد.

## 📋 فهرست مطالب

- [معرفی](#-معرفی)
- [ساختار پروژه](#-ساختار-پروژه)
- [لایه‌های معماری](#-لایههای-معماری)
- [الگوی CQRS](#-الگوی-cqrs)
  - [Contracts (قراردادها)](#-contracts-قراردادها)
  - [Abstractions در Application](#-abstractions-در-application)
  - [IAppMediator](#-iappmediator)
  - [مثال‌های عملی CQRS](#-مثالهای-عملی-cqrs)
- [Result Pattern](#-result-pattern)
  - [کلاس Result](#-کلاس-result)
  - [کلاس Error](#-کلاس-error)
  - [ErrorTypes](#-errortypes)
  - [ResultExtensions](#-resultextensions)
- [Domain Abstractions](#-domain-abstractions)
  - [Entity](#-entity)
  - [AggregateRoot](#-aggregateroot)
  - [ValueObject](#-valueobject)
  - [DomainEvent](#-domainevent)
  - [TypedId](#-typedid-strongly-typed-id)
  - [Auditable Contracts](#-auditable-contracts)
- [Validation](#-validation)
  - [FluentValidation Integration](#-fluentvalidation-integration)
  - [CollectionQueryRequestValidator](#-collectionqueryrequestvalidator)
  - [ValidationApiException](#-validationapiexception)
- [Exception Handling](#-exception-handling)
  - [GlobalExceptionHandler](#-globalexceptionhandler)
  - [ProblemDetailExtensions](#-problemdetailextensions)
- [Infrastructure](#-infrastructure)
  - [Entity Framework Configuration](#-entity-framework-configuration)
  - [AuditSaveChangesInterceptor](#-auditsavechangesinterceptor)
  - [Repository Pattern](#-repository-pattern)
- [Extensions](#-extensions)
- [نحوه استفاده](#-نحوه-استفاده)
- [مثال‌های کاربردی](#-مثالهای-کاربردی)

---

## 🎯 معرفی

این پروژه یک معماری زیرساختی کامل و قابل استفاده مجدد برای توسعه برنامه‌های وب با رویکرد **Domain-Driven Design (DDD)** و **Clean Architecture** است.

### ویژگی‌های کلیدی

- ✅ **Domain-Driven Design (DDD)**: پیاده‌سازی کامل الگوهای DDD
- ✅ **Clean Architecture**: جداسازی کامل لایه‌های مختلف
- ✅ **CQRS Pattern**: جداسازی Command و Query با Wolverine
- ✅ **Result Pattern**: مدیریت خطاها بدون Exception
- ✅ **ProblemDetails**: پاسخ‌های استاندارد RFC 7807
- ✅ **Strongly Typed IDs**: شناسه‌های قوی‌تایپ برای امنیت بیشتر
- ✅ **Audit Trail**: ردیابی کامل تغییرات
- ✅ **Soft Delete**: حذف منطقی رکوردها
- ✅ **Concurrency Control**: کنترل همزمانی با RowVersion
- ✅ **Domain Events**: مدیریت رویدادهای دامنه
- ✅ **FluentValidation**: اعتبارسنجی با قوانین روان

---

## 📁 ساختار پروژه

<div dir="ltr">

```
sample-ddd-clean-arch/
├── src/
│   ├── core/
│   │   ├── Architecture.Domain/          # لایه دامنه
│   │   └── Architecture.Application/     # لایه کاربردی
│   ├── Architecture.Infrastructure/      # لایه زیرساخت
│   ├── Architecture.Presentation/        # لایه ارائه (API)
│   └── Architecture.Shared/              # کتابخانه مشترک
```

</div>

---

## 🏛️ لایه‌های معماری

### 1️⃣ Architecture.Shared (کتابخانه مشترک)

این پروژه شامل **Contract ها**، **Abstraction ها** و ابزارهای مشترک است:
<div dir="ltr">

```
Architecture.Shared/
├── Commons/
│   ├── CQRS/Contracts/      # قراردادهای CQRS
│   │   ├── IAppMessage.cs
│   │   ├── ICommand.cs
│   │   ├── IQuery.cs
│   │   ├── IEvent.cs
│   │   └── IAppMediator.cs
│   ├── Result/              # الگوی Result
│   │   ├── Result.cs
│   │   ├── Error.cs
│   │   ├── ErrorTypes.cs
│   │   └── ResultExtensions.cs
│   ├── PaginationAction/    # ابزارهای صفحه‌بندی
│   └── Extensions/          # متدهای الحاقی
```

</div>


### 2️⃣ Architecture.Domain (لایه دامنه)

قلب اصلی سیستم - شامل Entity ها، Value Object ها، Aggregate Root ها و Domain Event ها:

<div dir="ltr">

```
Architecture.Domain/
├── Abstractions/
│   ├── Contracts/           # اینترفیس‌های پایه
│   │   ├── IEntity.cs
│   │   ├── IAggregateRoot.cs
│   │   ├── IValueObject.cs
│   │   ├── IDomainEvent.cs
│   │   ├── ITypedId.cs
│   │   ├── IAuditableProps.cs
│   │   └── IRowVersionProps.cs
│   ├── Entity.cs            # کلاس پایه Entity
│   ├── AggregateRoot.cs     # کلاس پایه Aggregate Root
│   ├── ValueObject.cs       # کلاس پایه Value Object
│   └── DomainEvent.cs       # کلاس پایه Domain Event
└── Orders/                  # ماژول سفارشات (مثال)
```

</div>


### 3️⃣ Architecture.Application (لایه کاربردی)

Use Case ها و Handler های CQRS:

<div dir="ltr">

```
Architecture.Application/
├── Abstractions/
│   ├── CommandRequest.cs    # کلاس پایه Command
│   ├── QueryRequest.cs      # کلاس پایه Query
│   ├── Event.cs             # کلاس پایه Event
│   ├── AppMediator.cs       # پیاده‌سازی Mediator
│   ├── Validators/          # Validator های پایه
│   ├── Middlewares/         # Middleware ها
│   └── Exceptions/          # Exception های سفارشی
└── UseCases/
    └── Orders/              # Use Case های سفارشات
        ├── Create/
        ├── Update/
        ├── Delete/
        └── Filter/
```

</div>


### 4️⃣ Architecture.Infrastructure (لایه زیرساخت)

پیاده‌سازی‌های فنی:

<div dir="ltr">

```
Architecture.Infrastructure/
├── Persistence/
│   ├── EF/
│   │   ├── DbContext/
│   │   ├── Configurations/
│   │   ├── Interceptors/
│   │   └── Abstractions/
│   ├── Repositories/
│   └── Constants/
```

</div>


### 5️⃣ Architecture.Presentation (لایه ارائه)

Controller ها و Exception Handler:

<div dir="ltr">

```
Architecture.Presentation/
├── Controllers/
├── Middlewares/
│   └── GlobalExceptionHandler.cs
├── Commons/Extensions/
│   └── ProblemDetailExtensions.cs
└── Configurations/
```

</div>

---

## ⚡ الگوی CQRS

### 🔷 Contracts (قراردادها)

#### IAppMessage
<div dir="ltr">

```csharp
// اینترفیس نشانگر برای پیام‌های اپلیکیشن
public interface IAppMessage { }
```

</div>


#### ICommandRequest
<div dir="ltr">

```csharp
// برای Command هایی که نتیجه‌ای برنمی‌گردانند
public interface ICommandRequest : IAppMessage { }

// برای Command هایی که نتیجه برمی‌گردانند
public interface ICommandRequest<TResponse> : ICommandRequest
    where TResponse : notnull { }

// Handler برای Command بدون خروجی
public interface ICommandRequestHandler<TCommand>
    where TCommand : ICommandRequest
{
    Task<Result> Handle(TCommand command, CancellationToken cancellationToken);
}

// Handler برای Command با خروجی
public interface ICommandRequestHandler<TCommand, TResponse>
    where TCommand : ICommandRequest<TResponse>
    where TResponse : notnull
{
    Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellationToken);
}
```

</div>

#### IQueryRequest
<div dir="ltr">

```csharp
// برای Query های تکی
public interface IQueryRequest<TResponse> : IAppMessage
    where TResponse : notnull { }

// برای Query های لیستی با صفحه‌بندی
public interface ICollectionQueryRequest<TResponse> : IAppMessage, IPaginatable, ISortable, ISortableItems
    where TResponse : notnull { }

// Handler برای Query تکی
public interface IQueryRequestHandler<TQuery, TResponse>
    where TQuery : IQueryRequest<TResponse>
    where TResponse : notnull
{
    Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellationToken);
}

// Handler برای Query لیستی
public interface ICollectionQueryRequestHandler<TQuery, TResponse>
    where TQuery : ICollectionQueryRequest<TResponse>
    where TResponse : notnull
{
    Task<Result<ICollectionActionResponse<TResponse>>> Handle(TQuery query, CancellationToken cancellationToken);
}
```

</div>

#### IEvent
<div dir="ltr">

```csharp
// برای Event ها
public interface IEvent { }

// Handler برای Event
public interface IEventHandler<in TEvent>
    where TEvent : class, IEvent
{
    Task Handle(TEvent @event, CancellationToken cancellationToken);
}
```

</div>

---

### 🔷 Abstractions در Application

کلاس‌های پایه برای تعریف ساده‌تر Command و Query:

#### CommandRequest
<div dir="ltr">

```csharp
public abstract record CommandRequest : ICommandRequest
{
    protected CommandRequest() { }
}
```

</div>

#### QueryRequest
<div dir="ltr">

```csharp
// برای Query تکی
public abstract record QueryRequest<TResponse> : IQueryRequest<TResponse>
    where TResponse : notnull { }

// برای Query لیستی با صفحه‌بندی
public abstract record CollectionQueryRequest<TResponse> : ICollectionQueryRequest<TResponse>
    where TResponse : notnull
{
    public int PageIndex { get; init; }
    public int PageSize { get; init; }
    public string? SortBy { get; init; }
    public bool? SortDesc { get; init; }

    // تعریف فیلدهای مجاز برای مرتب‌سازی
    public string[] SortableItems() => ValidSortFields();
    protected virtual string[] ValidSortFields() => [];
}
```

</div>

#### Event
<div dir="ltr">

```csharp
public abstract record Event : IEvent
{
    protected Event() { }
}
```

</div>

---

### 🔷 IAppMediator

واسط مرکزی برای ارسال Command ها، Query ها و انتشار Event ها:

<div dir="ltr">

```csharp
public interface IAppMediator
{
    // ارسال Command بدون خروجی
    Task<Result> SendAsync(ICommandRequest request, CancellationToken cancellationToken = default);
    
    // ارسال Command با خروجی
    Task<Result<TResponse>> SendAsync<TResponse>(ICommandRequest<TResponse> request, CancellationToken cancellationToken = default)
        where TResponse : notnull;
    
    // ارسال Query تکی
    Task<Result<TResponse>> SendAsync<TResponse>(IQueryRequest<TResponse> request, CancellationToken cancellationToken = default)
        where TResponse : notnull;
    
    // ارسال Query لیستی
    Task<Result<ICollectionActionResponse<TResponse>>> SendAsync<TResponse>(ICollectionQueryRequest<TResponse> request, CancellationToken cancellationToken = default)
        where TResponse : notnull;
    
    // انتشار Event
    ValueTask PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent;
}
```

</div>

#### پیاده‌سازی AppMediator
<div dir="ltr">

```csharp
internal sealed class AppMediator(IMessageBus messageBus) : IAppMediator
{
    public Task<Result> SendAsync(ICommandRequest request, CancellationToken cancellationToken = default)
        => messageBus.InvokeAsync<Result>(request, cancellationToken);

    public Task<Result<TResponse>> SendAsync<TResponse>(ICommandRequest<TResponse> request, CancellationToken cancellationToken = default)
        where TResponse : notnull
        => messageBus.InvokeAsync<Result<TResponse>>(request, cancellationToken);

    // ... سایر متدها
}
```

</div>

---

### 📝 مثال‌های عملی CQRS

#### مثال 1: ایجاد Command ساده

**تعریف Command:**
<div dir="ltr">

```csharp
public sealed record CreateOrderCommand : CommandRequest;
```

</div>

**پیاده‌سازی Handler:**
<div dir="ltr">

```csharp
public sealed class CreateOrderCommandHandler : ICommandRequestHandler<CreateOrderCommand>
{
    private readonly IOrderRepository _orderRepository;

    public CreateOrderCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var order = Order.Create
            (
                orderNumber: $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Random.Shared.NextInt64(100, 100000)}",
                totalAmount: 56451.54M,
                status: OrderStatus.Draft
            );

            order.AddItem("Product Sample", 10, 1120.12M);

            await _orderRepository.CreateAsync(order, cancellationToken);
            return Result.Success();
        }
        catch
        {
            return Result.Failure(Error.Failure("error_code", "خطا در ایجاد سفارش"));
        }
    }
}
```

</div>

**استفاده در Controller:**
<div dir="ltr">

```csharp
[HttpGet("/api/orders/create")]
public async Task<IActionResult> Create(CancellationToken cancellationToken = default)
{
    var result = await mediator.SendAsync(new CreateOrderCommand(), cancellationToken);
    return result.ToEndpointResponse();
}
```

</div>

---

#### مثال 2: Command با پارامتر

**تعریف:**
<div dir="ltr">

```csharp
public sealed record DeleteOrderCommand(Guid Id) : CommandRequest;
```

</div>

**Handler:**
<div dir="ltr">

```csharp
public sealed class DeleteOrderCommandHandler : ICommandRequestHandler<DeleteOrderCommand>
{
    private readonly IOrderRepository _orderRepository;

    public async Task<Result> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var foundedOrder = await _orderRepository.GetAsync(request.Id, cancellationToken);
        
        if (foundedOrder is null)
            return Result.Failure(Error.NotFound("order_not_found", "سفارش یافت نشد."));

        await _orderRepository.DeleteAsync(foundedOrder, cancellationToken);
        return Result.Success();
    }
}
```

</div>

---

#### مثال 3: Query با صفحه‌بندی

**تعریف Query با Validator داخلی:**
<div dir="ltr">

```csharp
public sealed record FilterOrdersQuery
(
    string? OrderNumber = default,
    OrderStatus? Status = default
) : CollectionQueryRequest<OrderDTO>
{
    // تعریف فیلدهای مجاز برای مرتب‌سازی
    protected override string[] ValidSortFields()
        => [nameof(OrderNumber), nameof(Status)];

    // Validator
    public sealed class FilterOrdersQueryValidator : CollectionQueryRequestValidator<FilterOrdersQuery, OrderDTO>
    {
        protected override void Validations()
        {
            RuleFor(x => x.OrderNumber)
                .MaximumLength(12).WithMessage("شماره سفارش نمی‌تواند بیش از 12 کاراکتر باشد.");
        }
    }
}
```

</div>

**پیاده‌سازی Handler:**
<div dir="ltr">

```csharp
public sealed class FilterOrdersQueryHandler : ICollectionQueryRequestHandler<FilterOrdersQuery, OrderDTO>
{
    public async Task<Result<ICollectionActionResponse<OrderDTO>>> Handle(
        FilterOrdersQuery query, 
        CancellationToken cancellationToken)
    {
        var orders = await GetOrdersFromDatabase(query);
        return orders.ToPaginatedResult(query);
    }
}
```

</div>

---

## ✅ Result Pattern

### 🔷 کلاس Result

برای مدیریت نتایج عملیات بدون استفاده از Exception:
<div dir="ltr">

```csharp
public class Result
{
    // Properties
    public bool IsSuccess { get; init; }
    [JsonIgnore]
    public bool IsFailure => !IsSuccess;
    public Error? Error { get; init; }

    // Factory Methods
    public static Result Success() => new(true, Error.None);
    public static Result Failure(Error error) => new(false, error);
    public static Result<T> Success<T>(T data) => Result<T>.Success(data);
    public static Result<T> Failure<T>(Error error) => Result<T>.Failure(error);

    // Functional Methods
    public Result OnSuccess(Action action);
    public Result OnFailure(Action<Error> action);
    public T Match<T>(Func<T> onSuccess, Func<Error, T> onFailure);
}
```
</div>


### 🔷 کلاس Result<T>

برای عملیات‌هایی که داده برمی‌گردانند:
<div dir="ltr">

```csharp
public class Result<T> : Result
{
    public T? Data { get; private init; }

    // Factory Methods
    public static Result<T> Success(T data);
    public static new Result<T> Failure(Error error);

    // Functional Methods
    public Result<TOut> Map<TOut>(Func<T, TOut> mapper);
    public Result<TOut> Bind<TOut>(Func<T, Result<TOut>> binder);
    public Result<T> OnSuccess(Action<T> action);
    public new Result<T> OnFailure(Action<Error> action);
    public TOut Match<TOut>(Func<T, TOut> onSuccess, Func<Error, TOut> onFailure);
    public T? ValueOrDefault(T? defaultValue = default);
    public T ValueOrThrow();

    // Implicit Conversions
    public static implicit operator Result<T>(T value);
    public static implicit operator Result<T>(Error error);
}
```

</div>

---

### 🔷 کلاس Error

انواع خطاها با کدها و متادیتا:

<div dir="ltr">

```csharp
public record Error
{
    // Properties
    public string Code { get; }
    public string Message { get; }
    public ErrorTypes Type { get; }
    public object? MetaData { get; }

    // مقدار ثابت برای عدم وجود خطا
    public static readonly Error None = new(string.Empty, string.Empty, ErrorTypes.None);

    // Factory Methods
    public static Error Validation(string code, string message, object? metaData = null);
    public static Error Unauthorized(string code, string message, object? metaData = null);
    public static Error Forbidden(string code, string message, object? metaData = null);
    public static Error NotFound(string code, string message, object? metaData = null);
    public static Error Conflict(string code, string message, object? metaData = null);
    public static Error Failure(string code, string message, object? metaData = null);
    public static Error UnprocessableEntity(string code, string message, object? metaData = null);
    public static Error TooManyRequests(string code, string message, object? metaData = null);
    public static Error ServiceUnavailable(string code, string message, object? metaData = null);
    public static Error Custom(string code, string message, ErrorTypes type, object? metaData = null);
    public static Error FromException(Exception exception, string? code = null);

    // Implicit Conversion
    public static implicit operator Result(Error error) => Result.Failure(error);
}
```

</div>

---

### 🔷 ErrorTypes

انواع خطا که به HTTP Status Code مپ می‌شوند:
<div dir="ltr">

```csharp
public enum ErrorTypes
{
    None = 100,              // 200 OK
    Validation = 400,        // 400 Bad Request
    Unauthorized = 401,      // 401 Unauthorized
    Forbidden = 403,         // 403 Forbidden
    NotFound = 404,          // 404 Not Found
    Conflict = 409,          // 409 Conflict
    UnprocessableEntity = 422, // 422 Unprocessable Entity
    TooManyRequests = 429,   // 429 Too Many Requests
    Failure = 500,           // 500 Internal Server Error
    ServiceUnavailable = 503 // 503 Service Unavailable
}
```

</div>

---

### 🔷 ResultExtensions

متدهای الحاقی برای کار با Result در سناریوهای async:
<div dir="ltr">

```csharp
public static class ResultExtensions
{
    // Async Extensions
    public static async Task<Result<TOut>> MapAsync<TIn, TOut>(
        this Task<Result<TIn>> resultTask,
        Func<TIn, TOut> mapper);

    public static async Task<Result<TOut>> BindAsync<TIn, TOut>(
        this Task<Result<TIn>> resultTask,
        Func<TIn, Task<Result<TOut>>> binder);

    public static async Task<Result<T>> OnSuccessAsync<T>(
        this Task<Result<T>> resultTask,
        Func<T, Task> action);

    public static async Task<Result<T>> OnFailureAsync<T>(
        this Task<Result<T>> resultTask,
        Func<Error, Task> action);

    // Conversion Extensions
    public static Result<Unit> ToGenericResult(this Result result);
    public static Result ToResult<T>(this Result<T> result);

    // Validation Extensions
    public static Result<T> Ensure<T>(this Result<T> result, Func<T, bool> predicate, Error error);
    public static Result<T> EnsureNotNull<T>(this Result<T> result, string code, string message);

    // Collection Extensions
    public static Result Combine(params Result[] results);
    public static Result CombineAll(params Result[] results);

    // Tap Extensions (Side Effects)
    public static Result<T> Tap<T>(this Result<T> result, Action<T> action);
    public static async Task<Result<T>> TapAsync<T>(this Result<T> result, Func<T, Task> action);
}
```

</div>

---

### 📝 مثال‌های استفاده از Result

#### استفاده ساده:
<div dir="ltr">

```csharp
public async Task<Result> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
{
    var order = await _orderRepository.GetAsync(request.Id, cancellationToken);
    
    if (order is null)
        return Result.Failure(Error.NotFound("order_not_found", "سفارش یافت نشد."));

    await _orderRepository.DeleteAsync(order, cancellationToken);
    return Result.Success();
}
```

</div>

#### استفاده از Match:

<div dir="ltr">

```csharp
var result = await GetOrderAsync(orderId);

var message = result.Match(
    onSuccess: order => $"سفارش {order.OrderNumber} یافت شد",
    onFailure: error => $"خطا: {error.Message}"
);
```

</div>

#### استفاده از Map و Bind:
<div dir="ltr">

```csharp
var orderNumber = result
    .Map(order => order.OrderNumber)
    .ValueOrDefault("نامشخص");

var validatedResult = result
    .Bind(order => ValidateOrder(order));
```

</div>

#### استفاده از OnSuccess و OnFailure:
<div dir="ltr">

```csharp
result
    .OnSuccess(order => Console.WriteLine($"موفق: {order.Id}"))
    .OnFailure(error => Console.WriteLine($"خطا: {error.Code}"));
```

</div>

#### استفاده از Ensure:
<div dir="ltr">

```csharp
var result = await GetOrderAsync();
result.Ensure(
    order => order.Status == OrderStatus.Draft,
    Error.Validation("invalid_status", "سفارش باید در وضعیت پیش‌نویس باشد")
);
```

</div>

---

## 🏛️ Domain Abstractions

### 🔷 Entity

کلاس پایه برای تمام Entity ها:
<div dir="ltr">

```csharp
public abstract class Entity<TId> : IEntity<TId>, IRowVersionProps
    where TId : notnull
{
    public long DbId { get; private set; }        // کلید دیتابیسی (Auto-increment)
    public TId Id { get; protected init; }         // شناسه دامنه (Domain ID)
    public byte[] RowVersion { get; private set; } // برای Concurrency Control

    public override bool Equals(object? obj);
    public override int GetHashCode();
}
```

</div>


### 🔷 TrackableEntity

برای Entity هایی که نیاز به Audit دارند:
<div dir="ltr">

```csharp
public abstract class TrackableEntity<TId> : Entity<TId>, IAuditableProps
{
    public Guid CreatorAccountId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public Guid? ModifierAccountId { get; private set; }
    public DateTime? ModifiedAt { get; private set; }
    public Guid? DeleterAccountId { get; private set; }
    public DateTime? DeletedAt { get; private set; }
    public RecordState RecordState { get; private set; }

    void ICreatableProps.SetCreated(Guid operatorId, DateTime now);
    void IModifiableProps.SetModified(Guid operatorId, DateTime now);
    void ISoftDeletableProps.SetDeleted(Guid operatorId, DateTime now);
}
```

</div>


---

### 🔷 AggregateRoot

کلاس پایه برای Aggregate Root ها با پشتیبانی از Domain Events:
<div dir="ltr">

```csharp
public abstract class AggregateRoot<TId> : Entity<TId>, IAggregateRoot
    where TId : struct, ITypedId
{
    private readonly List<IDomainEvent> _domainEvents = [];

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void RaiseDomainEvent(IDomainEvent domainEvent);
    public void ClearDomainEvents();
}

// نسخه Trackable
public abstract class TrackableAggregateRoot<TId> : TrackableEntity<TId>, IAggregateRoot
    where TId : struct, ITypedId { }
```

</div>


---

### 🔷 ValueObject

کلاس پایه برای Value Object ها:
<div dir="ltr">

```csharp
public abstract class ValueObject : IValueObject
{
    protected abstract IEnumerable<object?> GetEqualityComponents();

    public override bool Equals(object? obj);
    public override int GetHashCode();
}
```

</div>


---

### 🔷 DomainEvent

کلاس پایه برای Domain Event ها:
<div dir="ltr">

```csharp
public abstract record DomainEvent : IDomainEvent
{
    public DateTime OccurredOn => DateTime.UtcNow;
}

// مثال:
public sealed record OrderCreatedDomainEvent(OrderId OrderId) : DomainEvent;
public sealed record OrderSubmittedDomainEvent(OrderId OrderId, decimal TotalAmount) : DomainEvent;
```

</div>


---

### 🔷 TypedId (Strongly Typed Id)

برای جلوگیری از اشتباه در استفاده از ID ها:

<div dir="ltr">

```csharp
public readonly record struct OrderId(Guid Value) : ITypedId<Guid>
{
    public static OrderId New() => new(Guid.CreateVersion7());

    public override string ToString() => Value.ToString();

    public static implicit operator OrderId(Guid value) => new(value);
    public static implicit operator Guid(OrderId orderId) => orderId.Value;
}
```

</div>


**مزایا:**
- Type Safety کامل - نمی‌توانید OrderId را به جای CustomerId بفرستید
- خوانایی بهتر کد
- IntelliSense بهتر

---

### 🔷 Auditable Contracts

<div dir="ltr">

```csharp
// ایجاد
public interface ICreatableProps
{
    Guid CreatorAccountId { get; }
    DateTime CreatedAt { get; }
    void SetCreated(Guid operatorId, DateTime now);
}

// ویرایش
public interface IModifiableProps
{
    Guid? ModifierAccountId { get; }
    DateTime? ModifiedAt { get; }
    void SetModified(Guid operatorId, DateTime now);
}

// حذف منطقی
public interface ISoftDeletableProps
{
    Guid? DeleterAccountId { get; }
    DateTime? DeletedAt { get; }
    void SetDeleted(Guid operatorId, DateTime now);
}

// ترکیب همه
public interface IAuditableProps : ICreatableProps, IModifiableProps, ISoftDeletableProps
{
    RecordState RecordState { get; }
}
```

</div>


---

### 📝 مثال: تعریف Aggregate Root
<div dir="ltr">

```csharp
public sealed class Order : TrackableAggregateRoot<OrderId>
{
    private readonly List<OrderItem> _items = new();

    public string OrderNumber { get; private set; }
    public decimal TotalAmount { get; private set; }
    public OrderStatus Status { get; private set; }
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    // Private constructor for EF
    private Order() : base(default) { }

    // Factory method
    public static Order Create(string orderNumber, decimal totalAmount, OrderStatus status)
    {
        var order = new Order(OrderId.New(), orderNumber, totalAmount, status);
        order.RaiseDomainEvent(new OrderCreatedDomainEvent(order.Id));
        return order;
    }

    // Domain methods
    public void AddItem(string productName, int quantity, decimal price)
    {
        EnsureDraft();
        var item = OrderItem.Create(DbId, productName, quantity, price);
        _items.Add(item);
        RecalculateTotal();
    }

    public void Submit()
    {
        EnsureDraft();
        if (!_items.Any())
            throw new InvalidOperationException("Order has no items");

        Status = OrderStatus.Submitted;
        RaiseDomainEvent(new OrderSubmittedDomainEvent(Id, TotalAmount));
    }

    private void EnsureDraft()
    {
        if (Status != OrderStatus.Draft)
            throw new InvalidOperationException("Only draft orders can be modified");
    }
}
```

</div>

---

## 🛡️ Validation

### 🔷 FluentValidation Integration

پروژه از **FluentValidation** با **Wolverine** برای اعتبارسنجی خودکار استفاده می‌کند.

**تنظیمات در AppServiceConfiguration:**
<div dir="ltr">

```csharp
public static void ConfigureApplicationServices(this WebApplicationBuilder builder)
{
    // ثبت Validator ها
    builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

    builder.Services.AddWolverine(opts =>
    {
        // فعال‌سازی FluentValidation
        opts.UseFluentValidation();

        // تنظیم Custom Failure Action
        opts.Services.AddSingleton(typeof(IFailureAction<>), typeof(CustomFailureValidationAction<>));
    });
}
```

</div>

---

### 🔷 CollectionQueryRequestValidator

Validator پایه برای Query های لیستی با اعتبارسنجی خودکار صفحه‌بندی:

<div dir="ltr">

```csharp
public abstract class CollectionQueryRequestValidator<TQuery, TResponse> : AbstractValidator<TQuery>
    where TQuery : CollectionQueryRequest<TResponse>
    where TResponse : notnull
{
    protected virtual int MaxPageSize => 50;
    protected virtual bool CanGetAll => true;

    protected CollectionQueryRequestValidator()
    {
        // اعتبارسنجی SortBy
        RuleFor(x => IsValidSort(x))
            .Equal(true).WithMessage(x => SortParameterValidation(x.SortableItems()));

        // اعتبارسنجی PageIndex و PageSize
        When(x => !(x.PageIndex == 0 && x.PageSize == 0 && CanGetAll), () =>
        {
            RuleFor(x => x.PageIndex)
                .GreaterThan(0).WithMessage("مقدار PageIndex نمی‌تواند کمتر از 1 باشد.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("مقدار PageSize نمی‌تواند کمتر از 1 باشد.")
                .LessThanOrEqualTo(MaxPageSize).WithMessage($"مقدار PageSize نمی‌تواند بیشتر از {MaxPageSize} باشد.");
        });

        Validations();
    }

    // متد abstract برای تعریف قوانین سفارشی
    protected abstract void Validations();
}
```

</div>


---

### 🔷 ValidationApiException

Exception سفارشی برای خطاهای اعتبارسنجی:

<div dir="ltr">

```csharp
public sealed record ValidationError(IReadOnlyDictionary<string, string[]> Errors);

public sealed class ValidationApiException : Exception
{
    public ValidationError Errors { get; }

    public ValidationApiException(ValidationError errors)
        : base("Validation failed.")
    {
        Errors = errors;
    }
}
```

</div>


---

### 🔷 CustomFailureValidationAction

تبدیل خطاهای FluentValidation به ValidationApiException:

<div dir="ltr">

```csharp
public class CustomFailureValidationAction<T> : IFailureAction<T>
{
    public void Throw(T message, IReadOnlyList<ValidationFailure> failures)
    {
        var errors = failures
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key, 
                g => g.Select(e => e.ErrorMessage).Distinct().ToArray()
            );
        throw new ValidationApiException(new ValidationError(errors));
    }
}
```

</div>

---

### 📝 مثال: تعریف Validator
<div dir="ltr">

```csharp
public sealed record FilterOrdersQuery
(
    string? OrderNumber = default,
    OrderStatus? Status = default
) : CollectionQueryRequest<OrderDTO>
{
    // تعریف فیلدهای مجاز برای مرتب‌سازی
    protected override string[] ValidSortFields()
        => [nameof(OrderNumber), nameof(Status)];

    // تعریف Validator داخل Query
    public sealed class FilterOrdersQueryValidator : CollectionQueryRequestValidator<FilterOrdersQuery, OrderDTO>
    {
        protected override void Validations()
        {
            RuleFor(x => x.OrderNumber)
                .MaximumLength(12).WithMessage("شماره سفارش نمی‌تواند بیش از 12 کاراکتر باشد.");
        }
    }
}
```

</div>


---

## 🚨 Exception Handling

### 🔷 GlobalExceptionHandler

مدیریت مرکزی Exception ها در API:
<div dir="ltr">


```csharp
internal sealed class GlobalExceptionHandler
(
    ILogger<GlobalExceptionHandler> logger,
    IHostEnvironment environment
) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (httpContext.Response.HasStarted)
            return false;

        logger.LogError(exception, "Unhandled exception occurred");

        switch (exception)
        {
            case ValidationApiException validationException:
                await HandleValidationException(httpContext, validationException, cancellationToken);
                return true;
            default:
                await HandleUnknownException(httpContext, exception, cancellationToken);
                return true;
        }
    }

    private async Task HandleValidationException(
        HttpContext httpContext,
        ValidationApiException exception,
        CancellationToken cancellationToken)
    {
        var problemDetails = ProblemDetailExtensions.ToValidationErrorProblemDetails(exception, httpContext);
        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
    }

    private async Task HandleUnknownException(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var problemDetails = ProblemDetailExtensions.ToExceptionProblemDetails(
            exception, 
            environment.IsDevelopment(), 
            httpContext);
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
    }
}
```
</div>


**ثبت در Program.cs:**
<div dir="ltr">

```csharp
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// ...

app.UseExceptionHandler();
```

</div>

---

### 🔷 ProblemDetailExtensions

تبدیل Result به ProblemDetails استاندارد (RFC 7807):
<div dir="ltr">

```csharp
public static class ProblemDetailExtensions
{
    // برای استفاده در Controller ها
    public static IActionResult ToEndpointResponse(this Result result);
    public static IActionResult ToEndpointResponse<T>(this Result<T> result);

    // برای استفاده در Minimal APIs
    public static IResult ToEndPointResponse(this Result result);
    public static IResult ToEndPointResponse<T>(this Result<T> result);

    // تبدیل Validation Exception
    public static ProblemDetails ToValidationErrorProblemDetails(
        ValidationApiException exception, 
        HttpContext? httpContext = null);

    // تبدیل Exception عمومی
    public static ProblemDetails ToExceptionProblemDetails(
        Exception exception, 
        bool showMoreDetailsToResponse, 
        HttpContext? httpContext = null);
}
```

</div>

---

### 📝 مثال Response های API

#### موفقیت‌آمیز با داده (200 OK):
<div dir="ltr">

```json
{
    "status": 200,
    "title": "OK",
    "detail": "The request has succeeded.",
    "type": "https://httpstatuses.com/200",
    "Data": {
        "id": "019b69d7-6959-707c-9c5a-e611a9d3e48c",
        "orderNumber": "ORD-20250101-12345"
    }
}
```

</div>

#### خطای اعتبارسنجی (400 Bad Request):
<div dir="ltr">

```json
{
    "status": 400,
    "title": "Validation Error",
    "detail": "One or more validation errors occurred.",
    "type": "https://httpstatuses.com/400",
    "Errors": {
        "order_number": ["شماره سفارش نمی‌تواند بیش از 12 کاراکتر باشد."]
    },
    "TraceId": "00-xxxx-yyyy-00",
    "Instance": "/api/orders/filter"
}
```

</div>

#### خطای NotFound (404):
<div dir="ltr">

```json
{
    "status": 404,
    "title": "Not Found",
    "detail": "The requested resource could not be found.",
    "type": "https://httpstatuses.com/404",
    "Errors": {
        "order_not_found": ["سفارش یافت نشد."]
    }
}
```

</div>

#### خطای سرور (500) - در محیط Development:
<div dir="ltr">

```json
{
    "status": 500,
    "title": "Internal Server Error",
    "type": "InvalidOperationException",
    "detail": "Only draft orders can be modified",
    "StackTrace": "   at Architecture.Domain.Orders.Order.EnsureDraft()...",
    "TraceId": "00-xxxx-yyyy-00",
    "Instance": "/api/orders/submit/xxx"
}
```

</div>

---

## 🔧 Infrastructure

### 🔷 Entity Framework Configuration

#### BaseEntityConfig
<div dir="ltr">

```csharp
internal abstract class BaseEntityConfig<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : class, IEntity
{
    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        ConfigurePrimaryKey(builder);      // DbId به عنوان Primary Key
        ConfigureDeletionProps(builder);   // Soft Delete Filter
        ConfigureRowVersionProps(builder); // RowVersion برای Concurrency
        ConfigureEntity(builder);          // تنظیمات سفارشی
    }

    public abstract void ConfigureEntity(EntityTypeBuilder<TEntity> builder);

    // برای TypedId ها
    protected virtual void ConfigureDomainId<TTypedId, TPrimitive>(EntityTypeBuilder<TEntity> builder)
        where TTypedId : struct, ITypedId<TPrimitive>
    {
        builder.Property(nameof(IEntity<>.Id))
               .IsRequired()
               .HasConversion(new TypedIdValueConverter<TTypedId, TPrimitive>())
               .ValueGeneratedNever();

        builder.HasIndex(nameof(IEntity<>.Id)).IsUnique();
    }
}
```

</div>

#### مثال Configuration:
<div dir="ltr">

```csharp
internal sealed class OrderEntityConfig : BaseEntityConfig<Order>
{
    public override void ConfigureEntity(EntityTypeBuilder<Order> builder)
    {
        builder.Metadata.SetSchema(EntitySchema.BASE);

        ConfigureDomainId<OrderId, Guid>(builder);

        builder.Property(x => x.OrderNumber)
               .HasColumnType(SqlTypes.VARCHAR)
               .HasMaxLength(30)
               .IsRequired();

        builder.Property(x => x.TotalAmount)
               .HasPrecision(18, 2)
               .IsRequired();

        builder.HasMany(x => x.Items)
               .WithOne(x => x.Order)
               .HasForeignKey(x => x.OrderDbId);
    }
}
```

</div>

---

### 🔷 AuditSaveChangesInterceptor

Interceptor برای ذخیره خودکار اطلاعات Audit:

<div dir="ltr">

```csharp
public sealed class AuditSaveChangesInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(...)
    {
        var utcNow = DateTime.UtcNow;
        foreach (var entry in eventData.Context.ChangeTracker.Entries())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    if (entry.Entity is ICreatableProps creator)
                        creator.SetCreated(_currentOperator.OperatorAccountId, utcNow);
                    break;
                case EntityState.Modified:
                    if (entry.Entity is IModifiableProps modifier)
                        modifier.SetModified(_currentOperator.OperatorAccountId, utcNow);
                    break;
                case EntityState.Deleted:
                    if (entry.Entity is ISoftDeletableProps remover)
                    {
                        remover.SetDeleted(_currentOperator.OperatorAccountId, utcNow);
                        entry.State = EntityState.Modified; // Soft Delete
                    }
                    break;
            }
        }
    }
}
```

</div>


---

### 🔷 QueryFilterExtensions

اعمال خودکار Soft Delete Filter:

<div dir="ltr">

```csharp
internal static void ApplySoftDeleteFilter<TEntity>(this EntityTypeBuilder<TEntity> builder)
    where TEntity : class
{
    if (!typeof(IAuditableProps).IsAssignableFrom(typeof(TEntity)))
        return;

    // فیلتر کردن رکوردهای با وضعیت Deleted
    builder.HasQueryFilter(e => e.RecordState != RecordState.Deleted);
}
```

</div>


---

### 🔷 Repository Pattern
<div dir="ltr">

```csharp
// Interface در Domain
public interface IOrderRepository
{
    Task CreateAsync(Order order, CancellationToken cancellationToken);
    Task UpdateAsync(Order order, CancellationToken cancellationToken);
    Task DeleteAsync(Order order, CancellationToken cancellationToken);
    Task<Order?> GetAsync(OrderId id, CancellationToken cancellationToken);
    Task<Order?> GetAsync(long dbId, CancellationToken cancellationToken);
}

// Implementation در Infrastructure
internal sealed class OrderRepository(ApplicationDbContext DbContext) : IOrderRepository
{
    public async Task CreateAsync(Order order, CancellationToken cancellationToken)
    {
        DbContext.Orders.Add(order);
        await DbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Order?> GetAsync(OrderId id, CancellationToken cancellationToken)
    {
        return await DbContext.Orders
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    // ...
}
```

</div>

---

## 🔗 Extensions

### StringExtensions
<div dir="ltr">

```csharp
public static class StringExtensions
{
    // تبدیل "CamelCase" به "camel_case"
    public static string ToUnderscoreCase(this string input)
        => string.Concat(input.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLowerInvariant();
}
```

</div>

### JsonKeyExtensions
<div dir="ltr">

```csharp
public static class JsonKeyExtensions
{
    // تبدیل رشته به فرمت کلید JSON
    public static string ToJsonKey(this string input);
}
```

</div>

### CollectionResponseExtensions
<div dir="ltr">

```csharp
public static class CollectionResponseExtensions
{
    // تبدیل لیست به نتیجه صفحه‌بندی شده
    public static Result<ICollectionActionResponse<T>> ToPaginatedResult<T>(
        this IEnumerable<T> source, 
        ICollectionQueryRequest<T> collectionQueryRequest)
        where T : notnull;
}
```

</div>

---

## 🚀 نحوه استفاده

### راه‌اندازی پروژه

1. **تنظیم Connection String:**

<div dir="ltr">

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=.;Database=ArchitectureDb;Trusted_Connection=True;TrustServerCertificate=True;"
     }
   }
   ```

</div>

2. **اجرای Migration:**

<div dir="ltr">

   ```bash
   dotnet ef database update --project src/Architecture.Infrastructure
   ```

</div>


3. **اجرای پروژه:**

<div dir="ltr">

   ```bash
   dotnet run --project src/Architecture.Presentation
   ```

</div>


---

### ایجاد یک Use Case جدید

#### 1. تعریف Command:
<div dir="ltr">

```csharp
public sealed record CreateProductCommand(
    string Name,
    decimal Price
) : CommandRequest;
```

</div>


#### 2. تعریف Validator (اختیاری):
<div dir="ltr">

```csharp
public sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("نام الزامی است.")
            .MaximumLength(100);

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("قیمت باید بزرگتر از صفر باشد.");
    }
}
```

</div>

#### 3. پیاده‌سازی Handler:
<div dir="ltr">

```csharp
public sealed class CreateProductCommandHandler : ICommandRequestHandler<CreateProductCommand>
{
    public async Task<Result> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var product = Product.Create(command.Name, command.Price);
        await _repository.CreateAsync(product, cancellationToken);
        return Result.Success();
    }
}
```

</div>

#### 4. اضافه کردن Endpoint:
<div dir="ltr">

```csharp
[HttpPost("/api/products")]
public async Task<IActionResult> Create([FromBody] CreateProductCommand command, CancellationToken cancellationToken)
{
    var result = await mediator.SendAsync(command, cancellationToken);
    return result.ToEndpointResponse();
}
```

</div>

---

## 💡 مثال‌های کاربردی

### ResponseController - نمونه انواع Response ها
<div dir="ltr">

```csharp
[ApiController]
[Route("api/[controller]")]
public sealed class ResponseController : ControllerBase
{
    [HttpGet("/api/responses/failure")]
    public IActionResult Failure()
        => Result.Failure(Error.Failure("Operation Failed", "Operation requested is failed.")).ToEndpointResponse();

    [HttpGet("/api/responses/forbidden")]
    public IActionResult Forbidden()
        => Result.Failure(Error.Forbidden("Access Denied", "You haven't access to this endpoint.")).ToEndpointResponse();

    [HttpGet("/api/responses/unauthorized")]
    public IActionResult UnauthorizedRequest()
        => Result.Failure(Error.Unauthorized("Unauthorized", "Unauthorized request.")).ToEndpointResponse();

    [HttpGet("/api/responses/validation")]
    public IActionResult Validation()
        => Result.Failure(Error.Validation("Item Validation", "Invalid Item value.")).ToEndpointResponse();

    [HttpGet("/api/responses/conflict")]
    public IActionResult ConflictItem()
        => Result.Failure(Error.Conflict("Duplicated Item", "Item is existed.")).ToEndpointResponse();

    [HttpGet("/api/responses/not-found")]
    public IActionResult NotFoundItem()
        => Result.Failure(Error.NotFound("Item Not found", "Item not found.")).ToEndpointResponse();

    [HttpGet("/api/responses/global-exception")]
    public IActionResult GlobalException()
        => throw new ApplicationException("Just for testing!");
}
```

</div>

---

## 🎯 خلاصه

این معماری به شما امکان می‌دهد:

✅ **Domain-Centric Development**: تمرکز بر منطق کسب‌وکار  
✅ **CQRS**: جداسازی Command و Query  
✅ **Result Pattern**: مدیریت خطاها بدون Exception  
✅ **ProblemDetails**: پاسخ‌های استاندارد RFC 7807  
✅ **Type Safety**: با Strongly Typed IDs  
✅ **Audit Trail**: ردیابی کامل تغییرات  
✅ **Soft Delete**: حذف منطقی با امکان بازیابی  
✅ **FluentValidation**: اعتبارسنجی با قوانین روان  
✅ **Clean Architecture**: جداسازی کامل لایه‌ها  
✅ **Testability**: امکان تست آسان هر لایه  

---

## 📞 ارتباط با من

[![LinkedIn][linkedin-shield]][linkedin-url]
[![Telegram][telegram-shield]][telegram-url]
[![WhatsApp][whatsapp-shield]][whatsapp-url]
[![Gmail][gmail-shield]][gmail-url]
![GitHub followers](https://img.shields.io/github/followers/mh-zolfaghari)

<p align="right">(<a href="#readme-top">بازگشت به بالا</a>)</p>

<!-- MARKDOWN LINKS & IMAGES -->
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?logo=linkedin&color=555
[linkedin-url]: https://www.linkedin.com/in/ronixa/

[telegram-shield]: https://img.shields.io/badge/-Telegram-black.svg?logo=telegram&color=fff
[telegram-url]: https://t.me/DanialDotNet

[whatsapp-shield]: https://img.shields.io/badge/-WhatsApp-black.svg?logo=whatsapp&color=fff
[whatsapp-url]: https://wa.me/989389043224

[gmail-shield]: https://img.shields.io/badge/-Gmail-black.svg?logo=gmail&color=fff
[gmail-url]: mailto:personal.mhz@gmail.com

</div>