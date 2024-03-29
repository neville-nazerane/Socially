﻿using Socially.Models;
using Socially.Server.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Server.ModelMappings
{
    public static class CommentMappingExtensions
    {

        public static DisplayCommentModel ToDisplayModel(this Comment comment, bool? isLikedByCurrent = null)
        {
            if (comment is null) return null;
            return new DisplayCommentModel
            {
                Id = comment.Id,
                Text = comment.Text,
                LikeCount = comment.LikeCount ?? 0,
                CreatorId = comment.CreatorId,
                CreatedOn = comment.CreatedOn.Value,
                IsLikedByCurrentUser = isLikedByCurrent,
                Comments = new List<DisplayCommentModel>()
            };
        }

    }
}
