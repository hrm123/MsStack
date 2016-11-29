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
        [Route("Contacts/{startpage}/{pageSize}")]
        [HttpGet]
        public ApiResponseBase<List<Contact>> GetAllContacts(int startpage, int pageSize)
        {
            //http://localhost:21395/api/contactsapp/contacts/1/10
            try
            {
                IUnitofWork uow = new UnitOfWork();
                int totalCount = 0;
                var data = uow.ContactRepository.GetPage(startpage, pageSize, ref totalCount);
                ApiResponseBase<List<Contact>> resp = new ApiResponseBase<List<Contact>>
                {
                    AddnlInfo = totalCount.ToString(),
                    PayLoad = data.ToList(),
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
                    StatusMessage = "Failed wih server error.",
                    Success = false
                };
                return resp;

            }
        }

        [Route("Groups/{startpage}/{pageSize}")]
        [HttpGet]
        public ApiResponseBase<List<Group>> GetAllGroups(int startpage, int pageSize)
        {
            try
            {
                //http://localhost:21395/api/contactsapp/Groups/1/10
                IUnitofWork uow = new UnitOfWork();
                int totalCount = 0;
                var data = uow.GroupsRepository.GetPage(startpage, pageSize, ref totalCount);
                ApiResponseBase<List<Group>> resp = new ApiResponseBase<List<Group>>
                {
                    AddnlInfo = totalCount.ToString(),
                    PayLoad = data.ToList(),
                    StatusMessage = "Success",
                    Success = true
                };
                return resp;
            }
            catch (Exception ex)
            {
                // TODO - log error
                ApiResponseBase<List<Group>> resp = new ApiResponseBase<List<Group>>
                {
                    StatusMessage = "Failed with server error.",
                    Success = false
                };
                return resp;

            }
        }

    }
}
