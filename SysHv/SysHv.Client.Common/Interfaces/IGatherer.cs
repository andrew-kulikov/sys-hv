namespace SysHv.Client.Common.Interfaces
{
    public interface IGatherer
    {
        /// <summary>
        /// Must return Json with info that it is gathering
        /// </summary>
        /// <returns></returns>
        string Gather();
    }
}
