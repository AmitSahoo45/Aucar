'use client'

import React, { useEffect, useState } from 'react'
import AuctionCard from './AuctionCard'
import AppPagination from '../components/AppPagination'

const getAuctionData = async () => {
    try {
        const response = await fetch('http://localhost:6001/search?pageSize=8')
        const data = await response.json()
        return data
    } catch (error) {
        console.error(error)
    }
}

const Listings = async () => {
    const data = await getAuctionData();

    return (
        <>
            {/* <Filters /> */}
            {data.totalCount === 0 ? (
                // <EmptyFilter showReset />
                <div>EmptyFilter</div>
            ) : (
                <>
                    <div className='grid grid-cols-4 gap-6'>
                        {data && data.results.map((auction: any) => (
                            <AuctionCard key={auction.id} auction={auction} />
                        ))}
                    </div>
                    <div className='flex justify-center mt-4'>
                        <AppPagination
                            // pageChanged={setPageNumber}
                            currentPage={1} pageCount={data.pageCount} />
                    </div>
                </>
            )}
        </>
    )
}

export default Listings