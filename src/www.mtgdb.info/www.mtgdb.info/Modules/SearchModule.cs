using System;
using System.Configuration;
using System.Linq;
using MtgDb.Info.Driver;
using Nancy;
using Nancy.ModelBinding;

namespace MtgDb.Info
{
    public class SearchModule : NancyModule
    {
        public Db magicdb = 
            new Db (ConfigurationManager.AppSettings.Get("api"));

        public IRepository repository = 
            new MongoRepository (ConfigurationManager.AppSettings.Get("db"));

        public SearchModule () 
        {
            Get ["/search", true] = async (parameters, ct) => {
                SearchModel model = new SearchModel();
                model.Planeswalker = ((Planeswalker)this.Context.CurrentUser);
                model.ActiveMenu = "search";

                //color eq blue and type m 'Creature' and description m 'flying' and convertedmanacost lt 3 and name m 'Cloud'
                return View["Search", model];
            };

            Post ["/search", true] = async (parameters, ct) => {
                SearchModel model = this.Bind<SearchModel>();
                model.Planeswalker = ((Planeswalker)this.Context.CurrentUser);
                UserCard [] walkerCards = null;

                try
                {
                    Card[] cards = magicdb.Search(model.Term, isComplex: model.Advanced);
                    model.ActiveMenu = "search";

                    cards = cards
                        .AsEnumerable()
                        .OrderBy(x => x.Name)
                        .ThenByDescending(x => x.ReleasedAt).ToArray();

                    if(model.Planeswalker != null)
                    {
                        int [] cardIds = cards.AsEnumerable().Select(c => c.Id).ToArray();
                        walkerCards = repository.GetUserCards(model.Planeswalker.Id,cardIds);
                    }

                    foreach(var c in cards)
                    {
                        CardInfo cardInfo = new CardInfo();

                        if(walkerCards != null && walkerCards.Length > 0)
                        {
                            cardInfo.Amount = walkerCards.AsEnumerable()
                                .Where(info => info.MultiverseId == c.Id)
                                .Select(info => info.Amount).FirstOrDefault();

                        }
                        else
                        {
                            cardInfo.Amount = 0;
                        }

                        cardInfo.Card = c;
                        model.Cards.Add(cardInfo);
                    }
                }
                catch(Exception e)
                {
                    model.Errors.Add(e.Message);
                }
               
                return View["Search", model];
            };
        }
    }
}

