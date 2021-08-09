using System;

namespace Card.Classes
{
    internal  class Card
    {
        public CardBase m_iCard = null;
        const string DefaultReader = "Gemplus USB Smart Card Reader 0";
        
        #region Card
        public void SelectICard()
        {
            try
            {

                if (m_iCard != null)
                    m_iCard.Disconnect(DISCONNECT.Unpower);

                m_iCard = new CardNative();


                m_iCard.OnCardInserted += new CardInsertedEventHandler(m_iCard_OnCardInserted);
                m_iCard.OnCardRemoved += new CardRemovedEventHandler(m_iCard_OnCardRemoved);
                m_iCard.StartCardEvents(DefaultReader);

            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// CardRemovedEventHandler
        /// </summary>
        private void m_iCard_OnCardRemoved()
        {
            try
            {

            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            } 
        }

        /// <summary>
        /// CardInsertedEventHandler
        /// </summary>
        private void m_iCard_OnCardInserted()
        {
            try
            {
            
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }

        #endregion
    }
}
