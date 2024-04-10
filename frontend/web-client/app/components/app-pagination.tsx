'use client'

import React, { useState } from 'react'
import { Pagination } from 'flowbite-react'

type Props = {
  pageNumber: number
  pageCount: number
  setPage: (page: number) => void
}

const AppPagination = ({ pageCount, pageNumber, setPage }: Props) => {
  return (
    <Pagination
      totalPages={pageCount}
      currentPage={pageNumber}
      onPageChange={(event) => setPage(event)}
      layout="pagination"
      showIcons={true}
      className="text-blue-500"
    />
  )
}

export default AppPagination
