'use client'

import React, { useState, useEffect } from 'react'
import AuctionCard from './auction-card'
import { Auction } from '@/types'
import { getData } from '../actions/auction-actions'
import AppPagination from '../components/app-pagination'

const Listings = () => {
  const [auctions, setAuctions] = useState<Auction[]>([])
  const [pageCount, setPageCount] = useState(0)
  const [pageNumber, setPageNumber] = useState(1)

  useEffect(() => {
    const fetch = async () => {
      const data = await getData(pageNumber)
      setAuctions(data.results)

      if (pageCount === 0) {
        setPageCount(data.pageCount)
      }
    }

    fetch().catch(console.error)
  }, [pageNumber])

  return (
    <>
      <div className="grid grid-cols-4 gap-6">
        {auctions &&
          auctions.map((auction) => (
            <AuctionCard auction={auction} key={auction.id} />
          ))}
      </div>
      <div className="flex justify-center mt-4">
        <AppPagination
          pageCount={pageCount}
          pageNumber={pageNumber}
          setPage={setPageNumber}
        />
      </div>
    </>
  )
}

export default Listings
