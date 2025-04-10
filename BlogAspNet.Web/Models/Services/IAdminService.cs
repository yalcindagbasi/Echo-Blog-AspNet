namespace BlogAspNet.Web.Models.Services;

public interface IAdminService
{
    Task<object> GetStatistics();
}