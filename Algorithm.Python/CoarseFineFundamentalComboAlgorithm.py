# QUANTCONNECT.COM - Democratizing Finance, Empowering Individuals.
# Lean Algorithmic Trading Engine v2.0. Copyright 2014 QuantConnect Corporation.
# 
# Licensed under the Apache License, Version 2.0 (the "License"); 
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
# 
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.

from clr import AddReference
AddReference("System.Core")
AddReference("System.Collections")
AddReference("QuantConnect.Common")
AddReference("QuantConnect.Algorithm")

#from System import *
from System.Collections.Generic import List
from QuantConnect import *
from QuantConnect.Algorithm import QCAlgorithm
from QuantConnect.Data.UniverseSelection import *


class CoarseFineFundamentalComboAlgorithm(QCAlgorithm):
    '''In this algorithm we demonstrate how to define a universe as a combination of use the coarse fundamental data and fine fundamental data'''
    def Initialize(self):
        '''Initialise the data and resolution required, as well as the cash and start-end dates for your algorithm. All algorithms must initialized.'''
        
        self.SetStartDate(2014,01,01)  #Set Start Date
        self.SetEndDate(2015,01,01)    #Set End Date
        self.SetCash(50000)            #Set Strategy Cash
        
        self.UniverseSettings.Resolution = Resolution.Daily        
        
        # this add universe method accepts two parameters:
        # - coarse selection function: accepts an IEnumerable<CoarseFundamental> and returns an IEnumerable<Symbol>
        # - fine selection function: accepts an IEnumerable<FineFundamental> and returns an IEnumerable<Symbol>
        self.AddUniverse(self.CoarseSelectionFunction, self.FineSelectionFunction)

        self.__numberOfSymbols = 5
        self.__numberOfSymbolsFine = 2
        self._changes = SecurityChanges.None


    # sort the data by daily dollar volume and take the top 'NumberOfSymbols'
    def CoarseSelectionFunction(self, coarse):
        # sort descending by daily dollar volume
        sortedByDollarVolume = sorted(coarse, key=lambda x: x.DollarVolume, reverse=True) 

        # return the symbol objects of the top entries from our sorted collection
        top5 = sortedByDollarVolume[:self.__numberOfSymbols]

        # we need to return only the symbol objects
        list = List[Symbol]()
        for x in top5:
            list.Add(x.Symbol)

        return list

    # sort the data by P/E ratio and take the top 'NumberOfSymbolsFine'
    def FineSelectionFunction(self, fine):
        # sort descending by P/E ratio
        sortedByPeRatio = sorted(fine, key=lambda x: x.ValuationRatios.PERatio, reverse=True)

        # take the top entries from our sorted collection
        topFine = sortedByPeRatio[:self.__numberOfSymbolsFine]

        list = List[Symbol]()
        for x in topFine:
            list.Add(x.Symbol)

        return list


    def OnData(self, data):
        # if we have no changes, do nothing
        if self._changes == SecurityChanges.None: return

        # liquidate removed securities
        for security in self._changes.RemovedSecurities:
            if security.Invested:
                self.Liquidate(security.Symbol)
         
        # we want 20% allocation in each security in our universe
        for security in self._changes.AddedSecurities:
            self.SetHoldings(security.Symbol, 0.2)    
 
        self._changes = SecurityChanges.None;


    # this event fires whenever we have changes to our universe
    def OnSecuritiesChanged(self, changes):
        self._changes = changes