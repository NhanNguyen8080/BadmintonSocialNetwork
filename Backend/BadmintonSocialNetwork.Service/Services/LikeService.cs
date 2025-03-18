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
    public interface ILikeService
    {
        Task<ResponseDTO<LikeVM>> LikeUnlikePost(Guid postId);
        Task<int> CountLikesOfAPost(Guid postId);
    }
    public class LikeService : ILikeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LikeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> CountLikesOfAPost(Guid postId)
        {
            try
            {
                var likedPost = await _unitOfWork.LikeRepository.Get(_ => _.PostId == postId);
                if (likedPost is null)
                {
                    return 0;
                }
                return likedPost.Count();
            } catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<ResponseDTO<LikeVM>> LikeUnlikePost(Guid postId)
        {
            var response = new ResponseDTO<LikeVM>();
            try
            {
                var likedPost = await _unitOfWork.PostRepository.GetByID(postId);
                if (likedPost is null)
                {
                    response.IsSuccess = false;
                    response.Message = $"Cannot find any post with {postId}";
                    response.Data = null;
                    return response;
                }

                var accountId = await GetCurrentAccountFromToken();
                var isLiked = (await _unitOfWork.LikeRepository.Get(x => x.AccountId == accountId && x.PostId == postId))
                                                               .FirstOrDefault();
                if (isLiked is null)
                {
                    var like = new Like
                    {
                        AccountId = accountId,
                        PostId = likedPost.Id
                    };

                    await _unitOfWork.LikeRepository.InsertAsync(like);
                    await _unitOfWork.SaveAsync();

                    response.IsSuccess = true;
                    response.Message = "Liked post!";
                    response.Data = _mapper.Map<LikeVM>(like);
                    return response;
                }
                else
                {
                    _unitOfWork.LikeRepository.Delete(isLiked);
                    await _unitOfWork.SaveAsync();

                    response.IsSuccess = true;
                    response.Message = "Unliked post!";
                    response.Data = null;
                    return response;
                }


            } catch (Exception ex)
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
