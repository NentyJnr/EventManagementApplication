using Microsoft.EntityFrameworkCore;
using NCSEvent.API.Commons.Responses;
using NCSEvent.API.DTO;
using NCSEvent.API.Entities;
using NCSEvent.API.Services.Interfaces;

namespace NCSEvent.API.Services.Implementations
{
    public class MembershipManagementService : IMembershipManagementService
    {
        private readonly AppDbContext _dbContext;

        public MembershipManagementService(AppDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<ServerResponse<MembershipManagement>> UploadMember(MembershipManagementDto request)
        {
            var response = new ServerResponse<MembershipManagement>();

            var uploadMember = new MembershipManagement();
            uploadMember.FirstName = request.FirstName;
            uploadMember.LastName = request.LastName;
            uploadMember.Email = request.Email;
            uploadMember.MemberShipCode = request.MemberShipCode;
            uploadMember.MemberShipType = request.MemberShipType;
            uploadMember.IsActive = request.IsActive;
            uploadMember.IsDeleted = request.IsDeleted;
            uploadMember.DateJoined = request.DateJoined;
            uploadMember.DateCreated = DateTime.Now;
            uploadMember.DateModified = DateTime.Now;


            await _dbContext.AddAsync(uploadMember);
            var res = await _dbContext.SaveChangesAsync() > 0;
            if (res)
            {
                response.SuccessMessage = "Member uploaded successfully";
                response.IsSuccessful = true;
                response.Data = uploadMember;
            }
            else
            {
                response.SuccessMessage = "Something went wrong member can not be uploaded at the moment: please try again";
                response.IsSuccessful = false;
            }

            return response;
        }

        public async Task<ServerResponse<List<MembershipManagement>>> GetAllMembers()
        {
            var response = new ServerResponse<List<MembershipManagement>>();
            var allMember = await _dbContext.UploadMembers.ToListAsync();

            response.SuccessMessage = allMember.Count > 0
                ? "List of members retrieved successfully"
                : "No members found.";

            response.IsSuccessful = true;
            response.Data = allMember;

            return response;
        }

        public async Task<ServerResponse<MembershipManagement>> GetMemberById(long memberId)
        {
            var response = new ServerResponse<MembershipManagement>();

            var member = await _dbContext.UploadMembers.FindAsync(memberId);

            if (member == null)
            {
                response.SuccessMessage = "member not found";
                response.IsSuccessful = false;
            }
            else
            {
                response.SuccessMessage = "member successfully retrieved";
                response.Data = member;
                response.IsSuccessful = true;
            }

            return response;
        }


        public async Task<ServerResponse<bool>> UpdateMember(long memberId, MembershipManagementDto request)
        {
            var response = new ServerResponse<bool>();


            var member = await _dbContext.UploadMembers.FindAsync(memberId);

            if (member != null)
            {
                member.DateJoined = request.DateJoined;
                member.DateCreated = DateTime.Now;
                member.DateModified = DateTime.Now;
                member.FirstName = request.FirstName;
                member.LastName = request.LastName;
                member.Email = request.Email;
                member.MemberShipCode = request.MemberShipCode;
                member.MemberShipType = request.MemberShipType;
                member.IsActive = request.IsActive;
                member.IsDeleted = request.IsDeleted;

                //_dbContext.Update(member);
                await _dbContext.SaveChangesAsync();

                response.SuccessMessage = "Member updated successfully";
                response.IsSuccessful = true;
            }
            else
            {
                response.SuccessMessage = "Member not found";
                response.IsSuccessful = false;
            }


            return response;
        }


        public async Task<ServerResponse<bool>> DeleteMember(long memberId)
        {
            var response = new ServerResponse<bool>();

            var member = await _dbContext.UploadMembers.FindAsync(memberId);

            if (member != null)
            {
                _dbContext.UploadMembers.Remove(member);
                await _dbContext.SaveChangesAsync();

                response.SuccessMessage = "Hotel deleted successfully";
                response.IsSuccessful = true;
            }
            else
            {
                response.SuccessMessage = "Hotel not found";
                response.IsSuccessful = false;
            }

            return response;
        }

        public async Task<ServerResponse<bool>> DeactivateUploadMember(long memberId)
        {
            var response = new ServerResponse<bool>();

            var member = await _dbContext.UploadMembers.FindAsync(memberId);
            if (member != null)
            {
                member.IsActive = false;

                _dbContext.Update(member);

                await _dbContext.SaveChangesAsync();

                response.SuccessMessage = "Upload member deactivated successfully";
                response.IsSuccessful = true;
            }
            else
            {
                response.SuccessMessage = "Member not found";
                response.IsSuccessful = false;
            }


            return response;
        }

        public async Task<ServerResponse<bool>> ActivateUploadMember(long memberId)
        {
            var response = new ServerResponse<bool>();

            var member = await _dbContext.UploadMembers.FindAsync(memberId);
            if (member != null)
            {
                member.IsActive = true;

                _dbContext.Update(member);

                await _dbContext.SaveChangesAsync();

                response.SuccessMessage = "Upload member activated successfully";
                response.IsSuccessful = true;
            }
            else
            {
                response.SuccessMessage = "Member not found";
                response.IsSuccessful = false;
            }


            return response;
        }

       
    }
}
