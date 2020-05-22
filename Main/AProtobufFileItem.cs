
namespace ALittle
{

    public class AProtobufFileItem : FileItem
    {
        public AProtobufFileItem(ProjectInfo project, ABnf abnf, string full_path, uint item_id, ABnfFile file)
            : base(project, abnf, full_path, item_id, file)
        {
        }

        public string GetPackage()
        {
            AProtobufFile file = m_file as AProtobufFile;
            if (file == null) return "";
            return file.GetPackage();
        }

        public AProtobufCustomInfo GetCustomInfo()
        {
            AProtobufFile file = m_file as AProtobufFile;
            return file.GetCustomInfo();
        }

        public void CollectReference()
        {
            AProtobufFile file = m_file as AProtobufFile;
            file.CollectReference();
        }
    }
}
