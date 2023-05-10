using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PayrollApp;
using PayrollApp.Data;
using PayrollApp.Data.DbInitial;
using PayrollApp.Services;
using System.Configuration;
using System.Net;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


var jwtData = builder.Configuration.GetSection("JWT");
var jwtIssuer = jwtData["validIssuer"];
var jwtAudience = jwtData["validAudience"];
var jwtSecret = jwtData["secret"];
var jwtExpiresIn = jwtData["expiresIn"];


builder.Services.AddAuthentication(x => {
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  x.DefaultChallengeScheme= JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

    }
).AddJwtBearer(opt => {

    opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))

    };

});

builder.Services.AddAuthorization();
builder.Services.AddApiVersioning(o =>
{
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    o.ReportApiVersions = true;
    o.ApiVersionReader = ApiVersionReader.Combine(
        new QueryStringApiVersionReader("api-version"),
        new HeaderApiVersionReader("X-Version"),
        new MediaTypeApiVersionReader("ver"));
});


//builder.Services.AddVersionedApiExplorer(
//    options =>
//    {
//        options.GroupNameFormat = "'v'VVV";
//        options.SubstituteApiVersionInUrl = true;
//    });



// Add services to the container.
builder.Services.AddControllersWithViews();


// Configure swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{


    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme =JwtBearerDefaults.AuthenticationScheme,// "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
{
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        new string[] { }
    }


      
});


   
}


    );

builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();


builder.Services.AddVersionedApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
});

///

builder.Services.AddDbContext<PayrollDbContext>(options => {

    options.UseSqlServer(builder.Configuration.GetConnectionString("PayrollConn"));
});

//builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<PayrollDbContext>();


builder.Services.AddIdentity<AppUser, AppRole>(options => { 

 
} ).AddDefaultUI().AddEntityFrameworkStores<PayrollDbContext>().AddDefaultTokenProviders();


builder.Services.ConfigureApplicationCookie(options =>
{

    options.ExpireTimeSpan= TimeSpan.FromMinutes(30);

    options.LoginPath = "/Identity/Account/Login";

    options.Events.OnRedirectToLogin = context =>
    {
        if (context.Request.Path.Value.Contains("/api/"))
        {
           
            context.Response.StatusCode = 401;

            return Task.CompletedTask;
        }
        else
        {
            context.Response.Redirect(context.RedirectUri);// context.RedirectUri = options.LoginPath;
        }

        return Task.CompletedTask;
       
    };
});


builder.Services.AddRazorPages();

builder.Services.AddScoped<ILookupManager, LookupManager>();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddTransient<IEmailSender, EmailSender>();

var app = builder.Build();




// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


//app.UseStatusCodePages(async context => {
//    var request = context.HttpContext.Request;
//    var response = context.HttpContext.Response;

//    if (response.StatusCode == (int)HttpStatusCode.Unauthorized)
//        // you may also check requests path to do this only for specific methods       
//        // && request.Path.Value.StartsWith("/specificPath")
//        if (!request.Path.Value.Contains("/api/"))
//        {
//            response.Redirect("/account/login");
//        }
//});

// Configure swagger
app.UseSwagger();

var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
app.UseSwaggerUI(c => {


    foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions.Reverse())
    {
        c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
            description.GroupName.ToUpperInvariant());
    }


});

app.UseHttpsRedirection();
app.UseStaticFiles();



app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.MapRazorPages();

//user manager  and Role manager is scoped services

using(var  scoped = app.Services.CreateScope())
{
    
    var roleManager = scoped.ServiceProvider.GetService<RoleManager<AppRole>>();
    var userManager = scoped.ServiceProvider.GetService<UserManager<AppUser>>();

    UsersAndRolesInitializer initializer = new UsersAndRolesInitializer(userManager, roleManager);

    await  initializer.Initialize();

}


app.Run();

