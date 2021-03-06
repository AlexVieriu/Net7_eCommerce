var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddControllers();

builder.Services.AddAuthentication(AuthCookie.CookieName)
                .AddCookie(AuthCookie.CookieName, config =>
                {
                    config.Cookie.Name = AuthCookie.CookieName;
                    config.LoginPath = "/login";
                });

builder.Services.AddAutoMapper(typeof(CustomerUI));

// Singleton : Caching Services, Global Configuration, Business Rules, HttpClients
// Persisting state that's useful for the runtime of the application
builder.Services.AddSingleton<ISql, Sql>();
builder.Services.AddSingleton<IProductRepository, ProductRepository>();
builder.Services.AddSingleton<IOrderRepository, OrderRepository>();

// Scoped: Persisting state throughtout application per request
builder.Services.AddScoped<IShoppingCart, ShoppingCartBase>();
builder.Services.AddScoped<IShoppingCartStateStore, ShoppingCartStateStore>();

// Transient: Database Access, File Access, Services that should dispose of their state
// When you need a fresh instance of an object every single time
builder.Services.AddTransient<IOrderService, OrderService>();

builder.Services.AddTransient<ISearchProductUseCase, SearchProductUseCase>();
builder.Services.AddTransient<IViewProductUseCase, ViewProductUseCase>();

builder.Services.AddTransient<IAddProductToCartUseCase, AddProductToCartUseCase>();

builder.Services.AddTransient<IViewShoppingCartUseCase, ViewShoppingCartUseCase>();
builder.Services.AddTransient<IUpdateQuantityUseCase, UpdateQuantityUseCase>();
builder.Services.AddTransient<IDeleteLineItemUseCase, DeleteLineItemUseCase>();

builder.Services.AddTransient<IPlaceOrderUseCase, PlaceOrderUseCase>();
builder.Services.AddTransient<IViewOrderConfirmationUseCase, ViewOrderConfirmationUseCase>();

builder.Services.AddTransient<IViewProcessedOrdersUseCase, ViewProcessedOrdersUseCase>();
builder.Services.AddTransient<IViewOutStandingOrdersUseCase, ViewOutStandingOrdersUseCase>();
builder.Services.AddTransient<IViewOrderDetailUseCase, ViewOrderDetailUseCase>();
builder.Services.AddTransient<IProcessOrderUseCase, ProcessOrderUseCase>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
