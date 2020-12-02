
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class AProtobufServiceRpcElement : ABnfNodeElement
	{
		public AProtobufServiceRpcElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_ServiceRpcName = false;
        private AProtobufServiceRpcNameElement m_cache_ServiceRpcName = null;
        public AProtobufServiceRpcNameElement GetServiceRpcName()
        {
            if (m_flag_ServiceRpcName) return m_cache_ServiceRpcName;
            m_flag_ServiceRpcName = true;
            foreach (var child in m_childs)
            {
                if (child is AProtobufServiceRpcNameElement)
                {
                    m_cache_ServiceRpcName = child as AProtobufServiceRpcNameElement;
                    break;
                }
            }
            return m_cache_ServiceRpcName;
        }
        private bool m_flag_ServiceRpcReq = false;
        private AProtobufServiceRpcReqElement m_cache_ServiceRpcReq = null;
        public AProtobufServiceRpcReqElement GetServiceRpcReq()
        {
            if (m_flag_ServiceRpcReq) return m_cache_ServiceRpcReq;
            m_flag_ServiceRpcReq = true;
            foreach (var child in m_childs)
            {
                if (child is AProtobufServiceRpcReqElement)
                {
                    m_cache_ServiceRpcReq = child as AProtobufServiceRpcReqElement;
                    break;
                }
            }
            return m_cache_ServiceRpcReq;
        }
        private bool m_flag_ServiceRpcRsp = false;
        private AProtobufServiceRpcRspElement m_cache_ServiceRpcRsp = null;
        public AProtobufServiceRpcRspElement GetServiceRpcRsp()
        {
            if (m_flag_ServiceRpcRsp) return m_cache_ServiceRpcRsp;
            m_flag_ServiceRpcRsp = true;
            foreach (var child in m_childs)
            {
                if (child is AProtobufServiceRpcRspElement)
                {
                    m_cache_ServiceRpcRsp = child as AProtobufServiceRpcRspElement;
                    break;
                }
            }
            return m_cache_ServiceRpcRsp;
        }
        private bool m_flag_ServiceRpcBody = false;
        private AProtobufServiceRpcBodyElement m_cache_ServiceRpcBody = null;
        public AProtobufServiceRpcBodyElement GetServiceRpcBody()
        {
            if (m_flag_ServiceRpcBody) return m_cache_ServiceRpcBody;
            m_flag_ServiceRpcBody = true;
            foreach (var child in m_childs)
            {
                if (child is AProtobufServiceRpcBodyElement)
                {
                    m_cache_ServiceRpcBody = child as AProtobufServiceRpcBodyElement;
                    break;
                }
            }
            return m_cache_ServiceRpcBody;
        }
        List<AProtobufKeyElement> m_list_Key = null;
        public List<AProtobufKeyElement> GetKeyList()
        {
            var list = new List<AProtobufKeyElement>();
            if (m_list_Key == null)
            {
                m_list_Key = new List<AProtobufKeyElement>();
                foreach (var child in m_childs)
                {
                    if (child is AProtobufKeyElement)
                        m_list_Key.Add(child as AProtobufKeyElement);
                }   
            }
            list.AddRange(m_list_Key);
            return list;
        }
        List<AProtobufStringElement> m_list_String = null;
        public List<AProtobufStringElement> GetStringList()
        {
            var list = new List<AProtobufStringElement>();
            if (m_list_String == null)
            {
                m_list_String = new List<AProtobufStringElement>();
                foreach (var child in m_childs)
                {
                    if (child is AProtobufStringElement)
                        m_list_String.Add(child as AProtobufStringElement);
                }   
            }
            list.AddRange(m_list_String);
            return list;
        }

	}
}