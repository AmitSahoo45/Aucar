'use client'

import React, { useEffect, useState } from 'react'

const getAuctionData = async () => {
    try {
        const res = await fetch('http://localhost:6001/search')

        if (!res.ok) {
            throw new Error('Failed to fetch data')
        }

        return await res.json()
    } catch (error) {
        console.error(error)
    }
}

const Listings = () => {
    const data = getAuctionData()
    console.log(data)

    return (
        <div>
            Listings
            <div>
                {JSON.stringify(data, null, 2)}
            </div>
        </div>
    )
}

export default Listings