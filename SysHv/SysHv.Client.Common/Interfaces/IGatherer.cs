namespace SysHv.Client.Common.Interfaces
{
    public interface IGatherer<T>
    {
        /// <summary>
        /// Must return Json with info that it is gathering
        /// </summary>
        /// <returns></returns>
        T Gather();
    }
}
