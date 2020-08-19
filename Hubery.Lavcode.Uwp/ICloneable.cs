namespace Hubery.Lavcode.Uwp
{
    /// <summary>
    /// 对象复制
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICloneable<T>
    {
        /// <summary>
        /// 深复制
        /// </summary>
        /// <returns></returns>
        T DeepClone();

        /// <summary>
        /// 浅复制
        /// </summary>
        /// <returns></returns>
        T ShallowClone();
    }
}
