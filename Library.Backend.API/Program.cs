using Library.Backend.API.DB;
using Library.Backend.API.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<LibrarySystemDatabase>(options=>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MainDB"));
});

//for using any frontend
builder.Services.AddCors(options=>{
    options.AddDefaultPolicy(policy =>{
        policy.AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin();
    });
});
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddIdentityApiEndpoints<LibraryUser>(Options=>{
    Options.Password.RequireDigit=false;
    Options.Password.RequiredLength=4;
    Options.Password.RequireLowercase=false;
    Options.Password.RequireUppercase=false;
    Options.Password.RequireNonAlphanumeric=false;
    
})
    .AddEntityFrameworkStores<LibrarySystemDatabase>();

var app = builder.Build();


app.UseAuthentication();
app.UseAuthorization();

app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //   app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapIdentityApi<LibraryUser>();

//book
app.MapGet("/books/list",(LibrarySystemDatabase db)=>{
    return db.Books.ToList();
});
app.MapPost("/books/add",(LibrarySystemDatabase db,Book book)=>{
    db.Books.Add(book);
    db.SaveChanges();
});

//member
app.MapGet("/members/list",(LibrarySystemDatabase db)=>{
    return db.Members.ToList();
});
app.MapPost("/members/list",(LibrarySystemDatabase db,Member member)=>{
    db.Members.Add(member);
    db.SaveChanges();
});

//borrow
app.MapGet("/borrows/list",(LibrarySystemDatabase db)=>{
    return db.Borrows.Include(m=>m.Book).Include(m=>m.Member).ToList();
});
app.MapPost("/borrows/list",(LibrarySystemDatabase db,Borrow borrow)=>{
    db.Borrows.Add(borrow);
    db.SaveChanges();
});


app.Run();

