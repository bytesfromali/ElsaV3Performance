using Elsa.EntityFrameworkCore.Extensions;
using Elsa.EntityFrameworkCore.Modules.Management;
using Elsa.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddRazorPages();

builder.Services.AddElsa(elsa =>
{
    elsa.UseWorkflowRuntime(runtime =>
    {
        runtime.UseAsyncWorkflowStateExporter();
    });
    elsa.UseIdentity(identity =>
    {
        identity.UseAdminUserProvider();
        identity.TokenOptions = options => options.SigningKey = "ZWxzYXYzLXBlcmZvcm1hbmNlLXRlc3Q=";
    });
    elsa.UseDefaultAuthentication(auth => auth.UseAdminApiKey());
    //elsa.UseDefaultAuthentication();
    elsa.UseWorkflowManagement(management => management.UseEntityFrameworkCore(ef => ef.UseSqlServer(builder.Configuration.GetConnectionString("Elsa"))));
    elsa.UseJavaScript();
    elsa.UseLiquid();
    elsa.UseWorkflowsApi();
    elsa.UseHttp(http => http.ConfigureHttpOptions = options => options.BasePath = "/wf");
});



var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseWorkflowsApi();
app.UseWorkflows();
app.MapControllers();
app.MapRazorPages();

app.Run();
