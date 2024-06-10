using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenTrail.Forms.ViewModel
{
    public class MainWindowViewModel
    {
        public ObservableCollection<NewsViewModel> NewsViewModels { get; set; }
        public ObservableCollection<EventViewModels> EventViewModels { get; set; }
        public ObservableCollection<EcologicalRecommendationsViewModels> EcologicalRecommendationsViewModels { get; set; }

        public MainWindowViewModel()
        {
            NewsViewModels = new ObservableCollection<NewsViewModel>();
            EventViewModels = new ObservableCollection<EventViewModels>();
            EcologicalRecommendationsViewModels = new ObservableCollection<EcologicalRecommendationsViewModels>();

            LoadData();
        }

        private void LoadData()
        {
            using (var dbContext = new GreanTrailEntities())
            {
                var news = dbContext.News.ToList();
                foreach (var item in news)
                {
                    NewsViewModels.Add(new NewsViewModel
                    {
                        Id = item.id_news,
                        Title = item.heading,
                        Text = item.text,
                        Autor = item.Users.full_name,
                        Date = (DateTime)item.data_time
                    });
                }

                var events = dbContext.Event.ToList();
                foreach (var item in events)
                {
                    EventViewModels.Add(new EventViewModels
                    {
                        Id = item.id_event,
                        Name = item.name,
                        Date = (DateTime)item.data_time
                    });
                }

                var ecologicalRecommendations = dbContext.EcologicalRecommendations.ToList();
                foreach (var item in ecologicalRecommendations)
                {
                    EcologicalRecommendationsViewModels.Add(new EcologicalRecommendationsViewModels
                    {
                        Id = item.id_recommendations,
                        Title = item.heading,
                        Text = item.text,
                        Autor = item.Users.full_name,
                    });
                }
            }
        }
    }
}
