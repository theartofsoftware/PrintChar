using System.Collections.Generic;
using System.IO;
using System.Linq;
using EventBasedProgramming.Binding;
using EventBasedProgramming.TestSupport;
using JetBrains.Annotations;
using PluginApi.Model;
using SenseOfWonder.Model.Serialization;

namespace SenseOfWonder.Model
{
	public class WonderRulesCharacter : JsonBackedCharacter<RulesEditingSystem, RulesData>
	{
		protected WonderRulesCharacter([NotNull] RulesEditingSystem system, [NotNull] FileInfo characterData)
			: base(system)
		{
			File = characterData;
			CreateCardCommand = new SimpleCommand(Always.Enabled, _CreateCard);
		}

		[NotNull]
		public List<CardData> CardData { get; private set; }

		[NotNull]
		public SimpleCommand CreateCardCommand { get; private set; }

		public static WonderRulesCharacter Create([NotNull] RulesEditingSystem system, [NotNull] IDataFile characterData)
		{
			var result = new WonderRulesCharacter(system, characterData.Location);
			return (WonderRulesCharacter) result.FinishCreate(characterData);
		}

		public static WonderRulesCharacter Load([NotNull] RulesEditingSystem system, [NotNull] IDataFile characterData)
		{
			var result = new WonderRulesCharacter(system, characterData.Location);
			return (WonderRulesCharacter) result.FinishLoad(characterData);
		}

		public override RulesData PersistableData
		{
			get
			{
				return new RulesData
				{
					Cards = CardData
				};
			}
		}

		public override void UpdateFrom(RulesData characterData)
		{
			CardData = characterData.Cards.ToList();
			Cards.Clear();
			CardData.Select(_WrapCardInView).Each(Cards.Add);
		}

		private void _CreateCard()
		{
			var newCard = new CardData()
			{
				Name = Name
			};
			CardData.Add(newCard);
			Cards.Add(_WrapCardInView(newCard));
		}

		private static WonderCardView _WrapCardInView(CardData c)
		{
			return new WonderCardView
			{
				DataContext = c
			};
		}
	}

	public class WonderCardsDesignData : WonderRulesCharacter
	{
		public WonderCardsDesignData()
			: base(new RulesEditingSystem(), new FileInfo("anything.wonder"))
		{
			Name = "Agrippan Disk";
		}
	}
}