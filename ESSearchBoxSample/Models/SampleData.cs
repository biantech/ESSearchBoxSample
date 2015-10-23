using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ESSearchBoxSample.Models
{
    public class SampleData : CreateDatabaseIfNotExists<SampleEntities>
    {
          protected override void Seed(SampleEntities context)
        {
            new List<DocumentModel>
            {
                new DocumentModel {Name = "Reliability", Text = "Reliability is improved if multiple redundant sites are used, which makes well-designed cloud computing suitable for business continuity and disaster recovery."},
                new DocumentModel {Name = "Scalability and Elasticity", Text = "Scalability and Elasticity via dynamic provisioning of resources on a fine-grained, self-service basis near real-time, without users having to engineer for peak loads."},
                new DocumentModel {Name = "Performance", Text = "Performance is monitored, and consistent and loosely coupled architectures are constructed using web services as the system interface."},
                new DocumentModel {Name = "Application programming interface", Text = "API accessibility to software that enables machines to interact with cloud software in the same way the user interface facilitates interaction between humans and computers. Cloud computing systems typically use REST-based APIs."},
                new DocumentModel {Name = "Cost", Text = "Cost is claimed to be reduced and in a public cloud delivery model capital expenditure is converted to operational expenditure.This is purported to lower barriers to entry, as infrastructure is typically provided by a third-party and does not need to be purchased for one-time or infrequent intensive computing tasks. Pricing on a utility computing basis is fine-grained with usage-based options and fewer IT skills are required for implementation (in-house).The e-FISCAL project's state of the art repository contains several articles looking into cost aspects in more detail, most of them concluding that costs savings depend on the type of activities supported and the type of infrastructure available in-house."},
                new DocumentModel {Name = "Device and location independence", Text = "Device and location independence enable users to access systems using a web browser regardless of their location or what device they are using (e.g., PC, mobile phone). As infrastructure is off-site (typically provided by a third-party) and accessed via the Internet, users can connect from anywhere."},
                new DocumentModel {Name = "Virtualization", Text = "Virtualization technology allows servers and storage devices to be shared and utilization be increased. Applications can be easily migrated from one physical server to another."},
                new DocumentModel {Name = "Agility", Text = "Agility improves with users' ability to re-provision technological infrastructure resources."},
                new DocumentModel {Name = "Security", Text = "Security could improve due to centralization of data, increased security-focused resources, etc., but concerns can persist about loss of control over certain sensitive data, and the lack of security for stored kernels.Security is often as good as or better than other traditional systems, in part because providers are able to devote resources to solving security issues that many customers cannot afford.However, the complexity of security is greatly increased when data is distributed over a wider area or greater number of devices and in multi-tenant systems that are being shared by unrelated users. In addition, user access to security audit logs may be difficult or impossible. Private cloud installations are in part motivated by users' desire to retain control over the infrastructure and avoid losing control of information security."},
                new DocumentModel {Name = "Maintenance", Text = "Maintenance of cloud computing applications is easier, because they do not need to be installed on each user's computer and can be accessed from different places."}
            
            }.ForEach(a => context.DocumentModels.Add(a));
        }
    }
}
