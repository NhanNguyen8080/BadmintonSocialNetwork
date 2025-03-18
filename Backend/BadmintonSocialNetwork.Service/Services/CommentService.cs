using AutoMapper;
using BadmintonSocialNetwork.Repository.Interfaces;
using BadmintonSocialNetwork.Repository.Models;
using BadmintonSocialNetwork.Service.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonSocialNetwork.Service.Services
{
    public interface ICommentService
    {
        Task<ResponseDTO<CommentVM>> PostComment(CommentCM commentCM);
        Task<ResponseDTO<CommentVM>> EditComment(Guid commentId, CommentUM commentUM);
        Task<ResponseDTO<CommentVM>> DeleteComment(Guid commentId);
        Task<ResponseDTO<List<CommentVM>>> GetCommentsOfAPost(Guid postId);
    }
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICloudinaryService _cloudinaryService;

        public CommentService(IUnitOfWork unitOfWork, IMapper mapper, ICloudinaryService cloudinaryService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<ResponseDTO<CommentVM>> DeleteComment(Guid commentId)
        {
            var response = new ResponseDTO<CommentVM>();
            try
            {
                var deletedComment = await _unitOfWork.CommentRepository.GetByID(commentId);
                if (deletedComment is null)
                {
                    response.IsSuccess = false;
                    response.Message = $"There is no comment with id: {commentId}";
                    response.Data = null;
                    return response;
                }

                _unitOfWork.CommentRepository.Delete(deletedComment);
                await _unitOfWork.SaveAsync();
                response.IsSuccess = true;
                response.Message = "Delete comment successfully!";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                response.Data = null;
                return response;
            }
        }

        public async Task<ResponseDTO<CommentVM>> EditComment(Guid commentId, CommentUM commentUM)
        {
            var response = new ResponseDTO<CommentVM>();
            try
            {
                var editedComment = await _unitOfWork.CommentRepository.GetByID(commentId);
                if (editedComment is null)
                {
                    response.IsSuccess = false;
                    response.Message = $"There is no comment with id: {commentId}";
                    response.Data = null;
                    return response;
                }
                editedComment.ImageLink = "";

                if (commentUM.ImageFile is not null)
                {
                    var uploadResult = await _cloudinaryService.UploadImageToCloudinaryAsync(commentUM.ImageFile);
                    if (uploadResult is null || uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        response.IsSuccess = false;
                        response.Message = "Upload image to cloudinary failed!";
                        response.Data = null;
                        return response;
                    }
                    editedComment.ImageLink = uploadResult.SecureUrl.AbsoluteUri;
                }

                editedComment.Content = commentUM.Content;
                editedComment.UpdatedAt = DateTime.Now;
                _unitOfWork.CommentRepository.Update(editedComment);
                await _unitOfWork.SaveAsync();
                response.IsSuccess = true;
                response.Message = "Edit comment successfully!";
                response.Data = _mapper.Map<CommentVM>(editedComment);
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                response.Data = null;
                return response;
            }
        }

        public async Task<ResponseDTO<List<CommentVM>>> GetCommentsOfAPost(Guid postId)
        {
            var response = new ResponseDTO<List<CommentVM>>();
            try
            {
                var commentsOfAPost = await _unitOfWork.CommentRepository.Get(_ => _.PostId.Equals(postId));
                response.IsSuccess = true;
                response.Message = $"There are comments with Post Id: {postId}";
                response.Data = _mapper.Map<List<CommentVM>>(commentsOfAPost);
                return response;

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                response.Data = null;
                return response;
            }
        }

        public async Task<ResponseDTO<CommentVM>> PostComment(CommentCM commentCM)
        {
            var response = new ResponseDTO<CommentVM>();
            try
            {
                var postedComment = new Comment
                {
                    PostId = commentCM.PostId,
                    ParentCommentID = commentCM.ParentCommentID,
                    AccountId = await GetCurrentAccountFromToken(),
                    Content = commentCM.Content,
                    CreatedAt = DateTime.Now,
                    ImageLink = ""
                };

                if (commentCM.ImageFile is not null)
                {
                    var uploadResult = await _cloudinaryService.UploadImageToCloudinaryAsync(commentCM.ImageFile);
                    if (uploadResult is null || uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        response.IsSuccess = false;
                        response.Message = "Upload image to cloudinary failed!";
                        response.Data = null;
                        return response;
                    }
                    postedComment.ImageLink = uploadResult.SecureUrl.AbsoluteUri;
                }

                await _unitOfWork.CommentRepository.InsertAsync(postedComment);
                await _unitOfWork.SaveAsync();
                response.IsSuccess = true;
                response.Message = "Post comment successfully!";
                response.Data = _mapper.Map<CommentVM>(postedComment);
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                response.Data = null;
                return response;
            }
        }

        public async Task<int> GetCurrentAccountFromToken()
        {
            var httpContext = new HttpContextAccessor().HttpContext;
            if (httpContext == null)
            {
                throw new InvalidOperationException("No active HTTP context found.");
            }

            var user = httpContext.User;
            if (user == null || !user.Identity.IsAuthenticated)
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            var accountIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
            if (accountIdClaim == null)
            {
                throw new InvalidOperationException("No account ID claim found in token.");
            }

            if (!int.TryParse(accountIdClaim.Value, out var accountId))
            {
                throw new InvalidOperationException("Invalid account ID claim value.");
            }

            return accountId;
        }
    }
}
