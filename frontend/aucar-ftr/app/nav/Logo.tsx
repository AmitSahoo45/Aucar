'use client'

import { useParamsStore } from '@/hooks/useParamsStore';
import Image from 'next/image';
import { usePathname } from 'next/navigation';
import { useRouter } from 'next/navigation';

const Logo = () => {
    const router = useRouter();
    const pathname = usePathname();

    function doReset() {
        if (pathname !== '/') router.push('/');
        reset();
    }

    const reset = useParamsStore(state => state.reset)

    return (
        <div onClick={doReset} className='flex items-center gap-2 text-2xl cursor-pointer'>
            <Image src='/aucar-logo.png' width={50} height={50} alt='Aucar' />
        </div>
    )
}

export default Logo;