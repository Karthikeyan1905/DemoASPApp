using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using UserManagement.Model.User;
using UserManagementModel.Model;
using DemoASPApp.model;


public class RegistrationModel : BaseModel
{
    private readonly HttpClientConnector _httpClientConnector;

    public RegistrationModel(HttpClientConnector httpClientConnector)
    {
        _httpClientConnector = httpClientConnector;
    }

    public void OnGet()
    {
        TempData.Keep("sessionUser");
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var form = Request.Form;
        string loginUsername = form["loginUsername"];

        var existingUsers = await _httpClientConnector.GetAsync<List<UserInfo>>(
            "http://localhost:5112/UserInfo/Select",
            new Dictionary<string, string> { { "status", "A" } }
        );

        if (existingUsers.Any(u => u.loginCredential?.loginUsername == loginUsername))
        {
            ViewData["EMsg"] = "Username already exists.";
            return Page();
        }

        var queryParams = new Dictionary<string, string>
    {
        { "userName", form["userName"] },
        { "email", form["email"] },
        { "fatherName", form["fatherName"] },
        { "motherName", form["motherName"] },
        { "aadhaarNumber", form["AadhaarNumber"] },
       { "panCardNo", string.IsNullOrWhiteSpace(form["panCardNo"]) ? "null" : form["panCardNo"] },
        { "PassportNo", string.IsNullOrWhiteSpace(form["passportNo"]) ? "null" : form["passportNo"] },

        { "status", "A" },
        { "createdby", null } 
    };

        var result = await _httpClientConnector.PostAsync<ResponseResult>(
            "http://localhost:5112/UserInfo/Insert", queryParams
        );

        long newUserId = 0;
        if (result?.returnvalue is JsonElement elem)
        {
            if (elem.ValueKind == JsonValueKind.Number && elem.TryGetInt64(out long id))
            {
                newUserId = id;
            }
        }
        else if (result?.returnvalue is long l)
        {
            newUserId = l;
        }
        else if (result?.returnvalue is int i)
        {
            newUserId = i;
        }

        if (newUserId > 0)
        {
            long createdBy = CurrentUser?.userID ?? newUserId;

            await InsertPhones(newUserId, createdBy);
            await InsertAddresses(newUserId, createdBy);
            await InsertEducations(newUserId, createdBy);
            await InsertExperiences(newUserId, createdBy);
            await InsertLoginCredential(newUserId, form["loginUsername"], form["loginPassword"], createdBy);

            return RedirectToPage("Index", new { id = newUserId });
        }

        ViewData["EMsg"] = "Failed to register user.";
        return Page();
    }

    private async Task InsertLoginCredential(long userId, string username, string password, long createdBy)
    {
        var queryParams = new Dictionary<string, string>
    {
        { "userID", userId.ToString() },
        { "loginUsername", username },
        { "loginPassword", password },
        { "status", "A" },
        { "createdBy", createdBy.ToString()}
    };

        await _httpClientConnector.PostAsync<bool>("http://localhost:5112/LoginCredential/Insert", queryParams);
    }
    private async Task InsertPhones(long userId, long createdBy)
    {
        var phones = GetPhonesFromCollection();

        foreach (var phone in phones)
        {
            var queryParams = new Dictionary<string, string>
        {
            { "userID", userId.ToString() },
            { "phoneno", phone.phoneno.ToString() },
            { "phoneType", phone.phoneType },
            { "status", "A" },           
            { "createdBy", createdBy.ToString() }         
        };

            await _httpClientConnector.PostAsync<bool>("http://localhost:5112/Phone/Insert", queryParams);
        }
    }

    private List<Phone> GetPhonesFromCollection()
    {
        var form = Request.Form;
        var phones = new List<Phone>();

        if (!string.IsNullOrWhiteSpace(form["primaryPhone"]))
        {
            phones.Add(new Phone
            {
                phoneno = long.TryParse(form["primaryPhone"], out var primary) ? primary : 0,
                phoneType = "Primary",
                status = "A",
                createdBy = 1
            });
        }

        if (!string.IsNullOrWhiteSpace(form["secondaryPhone"]))
        {
            phones.Add(new Phone
            {
                phoneno = long.TryParse(form["secondaryPhone"], out var secondary) ? secondary : 0,
                phoneType = "Secondary",
                status = "A",
                createdBy = 1
            });
        }

        return phones;
    }
    private async Task InsertAddresses(long userId, long createdBy)
    {
        var addresses = GetAddressesFromCollection();
        foreach (var address in addresses)
        {
            var queryParams = new Dictionary<string, string>
            {
                { "userid", userId.ToString() },
                { "addressType", address.addressType },
                { "permDoorNo", address.permDoorNo },
                { "street", address.street },
                { "town", address.town },
                { "district", address.district },
                { "state", address.state },
                { "country", address.country },
                { "pincode", address.pincode.ToString() },
                { "status", "A" },
                { "createdBy", createdBy.ToString() }
            };

            await _httpClientConnector.PostAsync<bool>("http://localhost:5112/Address/Insert", queryParams);
        }
    }

    private async Task InsertEducations(long userId, long createdBy)
    {
        var educations = GetEducationFromCollection();
        foreach (var edu in educations)
        {
            var queryParams = new Dictionary<string, string>
            {
                { "userID", userId.ToString() },
                { "instituteName", edu.instituteName },
                { "instituteAddress", edu.instituteAddress ?? "" },
                { "board", edu.board ?? "" },
                { "courseName", edu.courseName },
                { "percentage", edu.percentage.ToString() },
                { "startYear", edu.startYear.ToString() },
                { "status", "A" },
                { "createdBy", createdBy.ToString() }
            };

            await _httpClientConnector.PostAsync<bool>("http://localhost:5112/Education/Insert", queryParams);
        }
    }

    private async Task InsertExperiences(long userId, long createdBy)
    {
        var experiences = GetExperienceFromCollection();
        foreach (var exp in experiences)
        {
            var queryParams = new Dictionary<string, string>
            {
                { "userID", userId.ToString() },
                { "companyName", exp.companyName },
                { "companyAddress", exp.companyAddress },
                { "workingexperainceinMonths", exp.workingexperainceinMonths.ToString() },
                { "contactPersonName", exp.contactPersonName },
                { "contactPersonDesignation", exp.contactPersonDesignation },
                { "contactPersonPhone", exp.contactPersonPhone.ToString() },
                { "contactPersonEmail", exp.contactPersonEmail },
                { "status", "A" },
                { "createdBy", createdBy.ToString() }
            };

            await _httpClientConnector.PostAsync<bool>("http://localhost:5112/Experience/Insert", queryParams);
        }
    }

    private List<Phone> GetPhoneFromCollection()
    {
        var form = Request.Form;
        var phones = new List<Phone>();

        for (int i = 1; i <= 2; i++)
        {
            long phone = long.TryParse(form["phoneNumber_" + i], out var parsedPhone) ? parsedPhone : 0;
            string phoneType = i == 1 ? "Primary" : "Secondary";

            phones.Add(new Phone { phoneno = phone, phoneType = phoneType });
        }

        return phones;
    }

    private List<Address> GetAddressesFromCollection()
    {
        var form = Request.Form;

        var addresses = new List<Address>();

        addresses.Add(new Address
        {
            addressType = form["addressType"], 
            permDoorNo = form["permDoorNo"],
            street = form["street"],
            town = form["town"],
            district = form["district"],
            state = form["state"],
            country = form["country"],
            pincode = long.TryParse(form["pincode"], out var permPin) ? permPin : 0
        });

        if (!string.IsNullOrWhiteSpace(form["secondaryDoorNo"]))
        {
            addresses.Add(new Address
            {
                addressType = form["secondaryAddressType"], 
                permDoorNo = form["secondaryDoorNo"],
                street = form["secondaryStreet"],
                town = form["secondaryTown"],
                district = form["secondaryDistrict"],
                state = form["secondaryState"],
                country = form["secondaryCountry"],
                pincode = long.TryParse(form["secondaryPincode"], out var secPin) ? secPin : 0
            });
        }

        return addresses;
    }
    

    private List<Education> GetEducationFromCollection()
    {
        var form = Request.Form;

        return new List<Education>
    {
        new Education
        {
            instituteName = form["tenthCollege"],
            instituteAddress = form["tenthAddress"],
            board = form["tenthBoard"],
            courseName = form["tenthCourse"],
            percentage = float.TryParse(form["tenthPercentage"], out var tenthPct) ? tenthPct : 0,
            startYear = int.TryParse(form["tenthStartYear"], out var tenthYear) ? tenthYear : 0
        },
        new Education
        {
            instituteName = form["twelfthCollege"],
            instituteAddress = form["twelfthAddress"],
            board = form["twelfthBoard"],
            courseName = form["twelfthCourse"],
            percentage = float.TryParse(form["twelfthPercentage"], out var twelfthPct) ? twelfthPct : 0,
            startYear = int.TryParse(form["twelfthStartYear"], out var twelfthYear) ? twelfthYear : 0
        },
        new Education
        {
            instituteName = form["bachelorCollege"],
            instituteAddress = form["bachelorAddress"],
            board = form["bachelorUniversity"],
            courseName = form["bachelorCourse"],
            percentage = float.TryParse(form["bachelorPercentage"], out var bachelorPct) ? bachelorPct : 0,
            startYear = int.TryParse(form["bachelorStartYear"], out var bachelorYear) ? bachelorYear : 0
        },
        new Education
        {
            instituteName = form["masterCollege"],
            instituteAddress = form["masterAddress"],
            board = form["masterUniversity"],
            courseName = form["masterCourse"],
            percentage = float.TryParse(form["masterPercentage"], out var masterPct) ? masterPct : 0,
            startYear = int.TryParse(form["masterStartYear"], out var masterYear) ? masterYear : 0
        }
    };
    }

    private List<Experience> GetExperienceFromCollection()
    {
        var form = Request.Form;
        var experiences = new List<Experience>();

        if (form["hasExperience"] == "yes")
        {
            experiences.Add(new Experience
            {
                companyName = form["companyName"],
                companyAddress = form["companyAddress"],
                workingexperainceinMonths = int.TryParse(form["workingExperienceInMonths"], out var exp1Months) ? exp1Months : 0,
                contactPersonName = form["contactPersonName"],
                contactPersonDesignation = form["contactPersonDesignation"],
                contactPersonPhone = long.TryParse(form["contactPersonPhone"], out var exp1Phone) ? exp1Phone : 0,
                contactPersonEmail = form["contactPersonEmail"]
            });

            if (!string.IsNullOrWhiteSpace(form["company2Name"]))
            {
                experiences.Add(new Experience
                {
                    companyName = form["company2Name"],
                    companyAddress = form["company2Address"],
                    workingexperainceinMonths = int.TryParse(form["company2Duration"], out var exp2Months) ? exp2Months : 0,
                    contactPersonName = form["company2ContactName"],
                    contactPersonDesignation = form["company2Designation"],
                    contactPersonPhone = long.TryParse(form["company2Phone"], out var exp2Phone) ? exp2Phone : 0,
                    contactPersonEmail = form["company2Email"]
                });
            }
        }

        return experiences;
    }

}
