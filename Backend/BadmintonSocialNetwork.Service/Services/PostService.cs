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
    public interface IPostService
    {
        Task<ResponseDTO<PostVM>> UploadPost(PostCM postCM);
        Task<ResponseDTO<PostVM>> EditPost(Guid postId, PostUM postUM);
        Task<ResponseDTO<PostVM>> DeletePost(Guid postId);
        Task<ResponseDTO<List<PostVM>>> GetPosts(int pageIndex, int pageSize);
        Task<ResponseDTO<BookmarkVM>> SavePost(Guid postId);
    }
    public class PostService : IPostService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PostService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseDTO<PostVM>> DeletePost(Guid postId)
        {
            var response = new ResponseDTO<PostVM>();
            try
            {
                var deletedPost = await _unitOfWork.PostRepository.GetByID(postId);
                if (deletedPost is null)
                {
                    response.Data = null;
                    response.IsSuccess = false;
                    response.Message = $"Cannot find any post with {postId}";
                    return response;
                }

                _unitOfWork.PostRepository.Delete(deletedPost);
                await _unitOfWork.SaveAsync();

                response.IsSuccess = true;
                response.Data = _mapper.Map<PostVM>(deletedPost);
                response.Message = "Delete post successfully!";
                return response;
            } catch (Exception ex)
            {
                response.Message = ex.Message;
                response.IsSuccess = false;
                response.Data = null;
                return response;
            }
        }

        public async Task<ResponseDTO<PostVM>> EditPost(Guid postId, PostUM postUM)
        {
            var response = new ResponseDTO<PostVM>();
            try
            {
                var editedPost = await _unitOfWork.PostRepository.GetByID(postId);
                if (editedPost is null)
                {
                    response.Data = null;
                    response.IsSuccess = false;
                    response.Message = $"Cannot find any post with {postId}";
                    return response;
                }
                editedPost.UpdatedAt = DateTime.Now;
                editedPost.Content = postUM.Content;
                editedPost.ImageFile = postUM.ImageFile;
                editedPost.AppearedPlace = postUM.AppearedPlace;
                _unitOfWork.PostRepository.Update(editedPost);
                await _unitOfWork.SaveAsync();

                response.Data = _mapper.Map<PostVM>(editedPost);
                response.IsSuccess = true;
                response.Message = "Edit post successfully!";
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.IsSuccess = false;
                response.Data = null;
                return response;
            }
        }

        public async Task<ResponseDTO<List<PostVM>>> GetPosts(int pageIndex, int pageSize)
        {
            var response = new ResponseDTO<List<PostVM>>();
            try
            {
                var accountId = await GetCurrentAccountFromToken();
                var account = await _unitOfWork.AccountRepository.GetByID(accountId);
                var posts = await _unitOfWork.PostRepository.Get(_ => _.AppearedPlace.Contains(account.Address),
                                                                    _ => _.OrderByDescending(p => p.CreatedAt), "", pageIndex, pageSize);
                var postVMs = _mapper.Map<List<PostVM>>(posts);
                response.IsSuccess = true;
                response.Message = "Get posts successfully!";
                response.Data = postVMs;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.IsSuccess = false;
                response.Data = null;
                return response;
            }
        }

        public async Task<ResponseDTO<BookmarkVM>> SavePost(Guid postId)
        {
            var response = new ResponseDTO<BookmarkVM>();
            try
            {
                var savedPost = await _unitOfWork.PostRepository.GetByID(postId);

                if (savedPost is null)
                {
                    response.Data = null;
                    response.IsSuccess = false;
                    response.Message = $"Cannot find any post with {postId}";
                    return response;
                }

                var bookmark = new Bookmark
                {
                    AccountId = await GetCurrentAccountFromToken(),
                    PostId = postId
                };

                response.Data = _mapper.Map<BookmarkVM>(bookmark);
                response.IsSuccess = true;
                response.Message = "Save post successfully!";
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.IsSuccess = false;
                response.Data = null;
                return response;
            }
        }

        public async Task<ResponseDTO<PostVM>> UploadPost(PostCM postCM)
        {
            var response = new ResponseDTO<PostVM>();
            try
            {
                var uploadedPost = _mapper.Map<Post>(postCM);
                uploadedPost.CreatedAt = DateTime.Now;
                uploadedPost.Status = true;
                uploadedPost.AccountId = await GetCurrentAccountFromToken();
                await _unitOfWork.PostRepository.InsertAsync(uploadedPost);
                await _unitOfWork.SaveAsync();

                response.Data = _mapper.Map<PostVM>(uploadedPost);
                response.IsSuccess = true;
                response.Message = "Upload post successfully!";
                return response;
            } catch (Exception ex)
            {
                response.Message = ex.Message;
                response.IsSuccess = false;
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
