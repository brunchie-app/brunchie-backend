using brunchie_backend.DataBase;
using brunchie_backend.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.ComponentModel;
namespace brunchie_backend.Repositories
{
    public class FeedbackRepository:IFeedbackRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public FeedbackRepository(AppDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task Post (FeedbackDto feedback)
        {
            var order= await _context.Order.FirstOrDefaultAsync(o=>o.OrderId==feedback.OrderId);

            if (order == null)
            {
                throw new KeyNotFoundException("Wrong OrderId entered for feedback");
            }

            feedback.VendorId=order.VendorId;

            Feedback feedback1 = new Feedback
            {
                OrderId = feedback.OrderId,
                VendorId = feedback.VendorId,
                Rating = feedback.Rating,
                Comment = feedback.Comment,
                CreatedAt = DateTime.Now,
            };

            try
            {
                _context.Feedback.Add(feedback1);
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateException e)
            {
                throw new Exception(e.Message);
            }

            catch (Exception e)
            {
                throw;
            }


        }

        public async Task<List<FeedbackDto>> Get (string VendorId)
        {
            var feedbackList = await _context.Feedback
                                 .Where(f => f.VendorId == VendorId).ToListAsync();

            var feedbackDtoList = _mapper.Map<List<Feedback>, List<FeedbackDto>>(feedbackList);

            return feedbackDtoList;
                                                                       

            
        }
    }
}
