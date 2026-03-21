namespace Framework.BLL;

public interface IBLLBase<out TBLLContext, TDomainObject> : IBLLQueryBase<TDomainObject>, IBLLContextContainer<TBLLContext>;
