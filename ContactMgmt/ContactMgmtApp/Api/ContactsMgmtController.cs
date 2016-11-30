using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ContactMgmtApp.DAL.Model;
using ContactMgmtApp.DAL.interfaces;
using ContactMgmtApp.DAL.Repos;

namespace ContactMgmtApp.Api
{
    [RoutePrefix("api/contactsapp")]
    public class ContactsMgmtController : ApiController
    {
        [Route("GetContact/{id}")]
        [HttpGet]
        public Contact GetContact(int id)
        {
            try
            {
                IUnitofWork uow = new UnitOfWork();
                return uow.ContactRepository.Get(id);
            }
            catch (Exception ex)
            {
                // TODO - log error
                return null;
            }
        }

        [Route("DeleteContact/{id}")]
        [HttpPost]
        public bool DeleteContact(int id)
        {
            try
            {
                IUnitofWork uow = new UnitOfWork();
                uow.ContactRepository.Delete(id);
                uow.Save();
                return true;
            }
            catch (Exception ex)
            {
                // TODO - log error
                return false;
            }
        }


        [Route("AddContact")]
        [HttpPost]
        public ApiResponseBase<int> AddContact([FromBody]  Contact c)
        {
            try
            {
                IUnitofWork uow = new UnitOfWork();
                int totalCount = 0;
                
                if (c.GroupIdsTemp.Length > 0)
                {
                    HashSet<Group> grps = new HashSet<Group>();
                    foreach (string gid  in c.GroupIdsTemp.Split(','))
                    {
                        grps.Add(uow.GroupsRepository.Get(int.Parse(gid)));
                    }
                    c.Groups = grps;
                }

                if(c.ContactId >0)
                {
                    uow.ContactRepository.Update(c);
                }
                else
                {
                    uow.ContactRepository.Insert(c);
                }
                
                uow.Save();

                ApiResponseBase<int> resp = new ApiResponseBase<int>
                {
                    AddnlInfo = c.ContactId.ToString(),
                    PayLoad = c.ContactId,
                    StatusMessage = "Success",
                    Success = true
                };
                return resp;
            }
            catch (Exception ex)
            {
                // TODO - log error
                ApiResponseBase<int> resp = new ApiResponseBase<int>
                {
                    StatusMessage = "Failed with server error.",
                    Success = false
                };
                return resp;

            }
        }

        [Route("contacts")]
        [HttpGet]
        public ApiResponseBase<List<Contact>> GetAllContacts([FromUri] int page, [FromUri] int pageSize)
        {
            //http://localhost:21395/api/contactsapp/contacts/1/10
            try
            {
                page = page - 1; // kendo gives 1 based 
                IUnitofWork uow = new UnitOfWork();
                int totalCount = 0;
                List<Contact> data = uow.ContactRepository.GetPage(page, pageSize, ref totalCount).ToList();

                
                foreach(Contact c in data)
                {
                    if (c.Groups.Count > 0)
                    {
                        c.GroupIdsTemp = string.Join(",", c.Groups.Select(g => g.GroupId).ToList());
                    }
                }
                
                
                ApiResponseBase<List<Contact>> resp = new ApiResponseBase<List<Contact>>
                {
                    AddnlInfo =  totalCount.ToString(),
                    PayLoad = data ,
                    StatusMessage = "Success",
                    Success = true
                };
                return resp;
            }
            catch(Exception ex)
            {
                // TODO - log error
                ApiResponseBase<List<Contact>> resp = new ApiResponseBase<List<Contact>>
                {
                    StatusMessage = "Failed with server error.",
                    Success = false
                };
                return resp;

            }
        }

        [Route("Groups/{startpage}/{pageSize}")]
        [HttpGet]
        public List<Group> GetAllGroups(int startpage, int pageSize)
        {
            List<Group> grpList = new List<Group>();
            try
            {
                //http://localhost:21395/api/contactsapp/Groups/1/10
                IUnitofWork uow = new UnitOfWork();
                int totalCount = 0;
                grpList = uow.GroupsRepository.GetPage(startpage, pageSize, ref totalCount).ToList();
                
                return grpList;
            }
            catch (Exception ex)
            {
                // TODO - log error
                return grpList;

            }
        }

    }
}
