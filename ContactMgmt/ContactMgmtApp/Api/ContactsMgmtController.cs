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
                var data = uow.ContactRepository.GetPage(page, pageSize, ref totalCount).ToList();

                /*
                for(int i = 0; i < 10; i++)
                {
                    data.Add(new Contact { FirstName = "fn" + i, LastName = "ln" + i });
                }
                */
                
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
