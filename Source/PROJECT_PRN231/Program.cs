using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PROJECT_PRN231;
using PROJECT_PRN231.Controllers;
using PROJECT_PRN231.Models;
using System.Text;
using Microsoft.EntityFrameworkCore;
using PROJECT_PRN231.Interface;
using PROJECT_PRN231.Models;
using PROJECT_PRN231.Repository;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers().AddJsonOptions(x =>
				x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddSwaggerGen(options =>
{
	options.AddSecurityDefinition("Bearer", //Name the security scheme
	new OpenApiSecurityScheme
	{
		Description = "JWT Authorization header using the Bearer scheme.",
		Type = SecuritySchemeType.Http, //We set the scheme type to http since we're using bearer authentication
		Scheme = "bearer" //The name of the HTTP Authorization scheme to be used in the Authorization header. In this case "bearer".
	});

	options.AddSecurityRequirement(new OpenApiSecurityRequirement{
	{
		new OpenApiSecurityScheme{
			Reference = new OpenApiReference{
				Id = "Bearer", //The name of the previously defined security scheme.
                Type = ReferenceType.SecurityScheme
			}
		},new List<string>()
	}
});
});
builder.Services.Configure<AppSettings>(
	builder.Configuration.GetSection("ApplicationSettings"));
builder.Services.AddDbContext<ExamSystemContext>(option =>
{
	option.UseSqlServer(builder.Configuration.GetConnectionString("MyDB"));
});

//addScope
builder.Services.AddScoped<IExamRepository, ExamRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<IExamQuestionRepository, ExamQuestionRepository>();
builder.Services.AddScoped<IAnswerRepository, AnswerRepository>();
builder.Services.AddScoped<IUserExamResultRepository, UserExamResultRepository>();
builder.Services.AddScoped<IUserExamQuestionAnswerRepository, UserExamQuestionAnswerRepository>();

builder.Services.AddAuthentication(x =>
{
	x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}
).AddJwtBearer(x =>
{
	x.RequireHttpsMetadata = false;
	x.SaveToken = true;
	x.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuerSigningKey = true,
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["ApplicationSettings:Secret"])),
		ValidateIssuer = false,
		ValidateAudience = false
	};
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors(policy => policy.AllowAnyHeader()
					 .AllowAnyMethod()
					 .SetIsOriginAllowed(origin => true)
					 .AllowCredentials());
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
