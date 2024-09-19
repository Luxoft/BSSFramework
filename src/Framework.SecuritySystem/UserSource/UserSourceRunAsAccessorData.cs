using System.Linq.Expressions;

namespace Framework.SecuritySystem.UserSource;

public record UserSourceRunAsAccessorData<TUser>(Expression<Func<TUser, TUser?>> Path);
