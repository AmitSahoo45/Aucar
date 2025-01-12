'use client';

import React, { useState } from 'react'
import Image from 'next/image';

type Props = {
    imageUrl: string;
}

export default function CarImage({ imageUrl }: Props) {

    return (
        <Image
            src={imageUrl}
            fill
            alt='image of car'
            priority
            className={`object-cover group-hover:opacity-75 duration-700 ease-in-out grayscale-0 blur-0 scale-100`}
            sizes='(max-width: 768px) 100vw, (max-width: 1200px) 50vw, 25vw'
        />
    )
}