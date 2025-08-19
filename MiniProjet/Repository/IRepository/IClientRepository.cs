using Shared.Models;

namespace MiniProjet.Repository.IRepository
{
    public interface IResponsableSAVRepository
    {
        public List<ResponsableSAV> GetResponsableSAVs();

        public ResponsableSAV GetResponsableSAVById(int id);

        public ResponsableSAV AddResponsableSAV(ResponsableSAV ResponsableSAV);

        public ResponsableSAV UpdateResponsableSAV(ResponsableSAV ResponsableSAV);

    }
}
