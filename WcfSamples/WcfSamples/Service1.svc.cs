using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfSamples
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);

            /*
             * example call from postman
             * 
             * 
             Body - <?xml version="1.0" encoding="UTF-8" ?>
            <s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
            <s:Body>
            <GetData xmlns="http://tempuri.org/">
            <value>0</value>
            </GetData>
            </s:Body>
            </s:Envelope>
            Headers - 
              [{"key":"Content-Type","value":"text/xml","description":""}]
            [{"key":"SOAPAction","value":"http://tempuri.org/IService1/GetData","description":""}] 
             */

            /*
             Example config in Mulesoft
             <http:listener-config doc:name="HTTP Listener Configuration" host="localhost" name="HTTP_Listener_Configuration" port="8081" basePath="testwcf1"/>
            <http:request-config doc:name="HTTP Request Configuration" host="localhost" name="HTTP_Request_Configuration" port="49168"/>
            <flow name="soap-api-proxyFlow">
                <http:listener config-ref="HTTP_Listener_Configuration" path="/" doc:name="HTTP"/>
                <http:request config-ref="HTTP_Request_Configuration" doc:name="Send Requests to API" method="POST" path="/Service1.svc">
                    <http:request-builder>
                        <http:header headerName="Content-Type" value="text/xml"/>
                        <http:header headerName="SOAPAction" value="http://tempuri.org/IService1/GetData"/>
                    </http:request-builder>
                </http:request>
            </flow>

            then post req from postman
            http://localhost:8081/testwcf1
            with same body as above postman req.
             */
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}
