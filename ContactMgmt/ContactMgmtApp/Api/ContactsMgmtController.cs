﻿using System;
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
        [Route("AddContact")]
        [HttpPost]
        public ApiResponseBase<Contact> AddContact([FromBody]  Contact c)
        {
            try
            {
                IUnitofWork uow = new UnitOfWork();
                int totalCount = 0;
                if(c.ContactId >0)
                {
                    uow.ContactRepository.Update(c);
                }
                else
                {
                    uow.ContactRepository.Insert(c);
                }
                
                uow.Save();

                ApiResponseBase<Contact> resp = new ApiResponseBase<Contact>
                {
                    AddnlInfo = totalCount.ToString(),
                    PayLoad = c,
                    StatusMessage = "Success",
                    Success = true
                };
                return resp;
            }
            catch (Exception ex)
            {
                // TODO - log error
                ApiResponseBase<Contact> resp = new ApiResponseBase<Contact>
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
