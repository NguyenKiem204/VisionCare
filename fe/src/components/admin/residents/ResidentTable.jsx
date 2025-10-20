import React from "react";
import { formatDate } from "../../../utils/formatDate";
const ResidentTable = ({ residents, loading, onEdit, onDelete }) => (
  <table className="min-w-full divide-y divide-gray-200 dark:divide-gray-700 rounded-xl overflow-hidden shadow-lg">
    <thead className="bg-gray-100 dark:bg-gray-800">
      <tr>
        <th className="px-4 py-3 text-left text-xs font-semibold text-gray-700 dark:text-gray-200 uppercase tracking-wider border-b border-gray-200 dark:border-gray-700">
          ID
        </th>
        <th className="px-4 py-3 text-left text-xs font-semibold text-gray-700 dark:text-gray-200 uppercase tracking-wider border-b border-gray-200 dark:border-gray-700">
          FullName
        </th>
        <th className="px-4 py-3 text-left text-xs font-semibold text-gray-700 dark:text-gray-200 uppercase tracking-wider border-b border-gray-200 dark:border-gray-700">
          Dob
        </th>
        <th className="px-4 py-3 text-left text-xs font-semibold text-gray-700 dark:text-gray-200 uppercase tracking-wider border-b border-gray-200 dark:border-gray-700">
          Gender
        </th>
        <th className="px-4 py-3 text-left text-xs font-semibold text-gray-700 dark:text-gray-200 uppercase tracking-wider border-b border-gray-200 dark:border-gray-700">
          Phone
        </th>
        <th className="px-4 py-3 text-left text-xs font-semibold text-gray-700 dark:text-gray-200 uppercase tracking-wider border-b border-gray-200 dark:border-gray-700">
          Address
        </th>
        <th className="px-4 py-3 text-left text-xs font-semibold text-gray-700 dark:text-gray-200 uppercase tracking-wider border-b border-gray-200 dark:border-gray-700">
          Married
        </th>
        <th className="px-4 py-3 text-left text-xs font-semibold text-gray-700 dark:text-gray-200 uppercase tracking-wider border-b border-gray-200 dark:border-gray-700">
          RelationToHead
        </th>
        <th className="px-4 py-3 text-left text-xs font-semibold text-gray-700 dark:text-gray-200 uppercase tracking-wider border-b border-gray-200 dark:border-gray-700">
          Family
        </th>
        <th className="px-4 py-3 text-left text-xs font-semibold text-gray-700 dark:text-gray-200 uppercase tracking-wider border-b border-gray-200 dark:border-gray-700">
          Actions
        </th>
      </tr>
    </thead>
    <tbody>
      {loading ? (
        <tr>
          <td colSpan="7" className="text-center py-6">
            Đang tải...
          </td>
        </tr>
      ) : residents.length === 0 ? (
        <tr>
          <td colSpan="7" className="text-center py-6 text-gray-400">
            Không có dữ liệu
          </td>
        </tr>
      ) : (
        residents.map((resident, idx) => (
          <tr
            key={resident.id}
            className={`text-sm ${
              idx % 2 === 0
                ? "bg-white dark:bg-gray-900"
                : "bg-gray-50 dark:bg-gray-800"
            } border-b border-gray-100 dark:border-gray-800 hover:bg-blue-50 dark:hover:bg-gray-700 transition-colors duration-200`}
          >
            <td className="px-4 py-2 text-gray-800 dark:text-gray-100">
              {resident.id}
            </td>
            <td className="px-4 py-2 text-gray-800 dark:text-gray-100">
              {resident.fullName}
            </td>
            <td className="px-4 py-2 text-gray-800 dark:text-gray-100">
              {formatDate(resident.dateOfBirth)}
            </td>
            <td className="px-4 py-2 text-gray-800 dark:text-gray-100">
              {resident.gender}
            </td>
            <td className="px-4 py-2 text-gray-800 dark:text-gray-100">
              {resident.phone}
            </td>
            <td className="px-4 py-2 text-gray-800 dark:text-gray-100">
              {resident.address}
            </td>
            <td className="px-4 py-2 text-gray-800 dark:text-gray-100">
              {resident.merried ? "Có" : "Không"}
            </td>
            <td className="px-4 py-2 text-gray-800 dark:text-gray-100">
              {resident.relationToHead}
            </td>
            <td className="px-4 py-2 text-gray-800 dark:text-gray-100">
              {resident.familyCode}
            </td>
            <td className="px-4 py-2 flex gap-2">
              <button
                className="bg-blue-600 hover:bg-blue-700 text-white px-3 py-1 rounded shadow text-xs font-semibold transition"
                onClick={() => onEdit(resident)}
              >
                Sửa
              </button>
              <button
                className="bg-gray-200 dark:bg-gray-700 dark:text-white hover:bg-red-500 hover:text-white px-3 py-1 rounded shadow text-xs font-semibold transition"
                onClick={() => onDelete(resident.id)}
              >
                Xóa
              </button>
            </td>
          </tr>
        ))
      )}
    </tbody>
  </table>
);

export default ResidentTable;
