import { GetCategoryReferenceAsync } from '@/api/reqCategory.js'
import { ref } from 'vue'
import { PostContentAsync } from '@/api/reqContent.js'

const categoryRef = ref([])

const newArticle = ref({
  title: '',
  categoryId: '',
  descriptionHtml: '',
  description: '',
  article: '',
  additionalLink: ''
})

const errorInput = ref({
  title: { isError: false, message: '' },
  categoryId: { isError: false, message: '' },
  description: { isError: false, message: '' },
  article: { isError: false, message: '' }
})

async function GetCategoryReference() {
  const result = await GetCategoryReferenceAsync()
  if (result.is_success) {
    categoryRef.value = result.reference.map((item) => {
      return {
        id: item.category_id,
        name: item.category_name
      }
    })
  }
}

async function HandleSave(action) {
  const isValid = ValidateEmptyInput()
  if (!isValid) return false

  const postArticle = await PostContentAsync(
    action,
    newArticle.value.title,
    newArticle.value.categoryId,
    newArticle.value.descriptionHtml,
    JSON.stringify(newArticle.value.description),
    JSON.stringify(newArticle.value.article),
    newArticle.value.additionalLink
  )

  if (!postArticle.is_success) {
    if (postArticle.error.statuscode === 400) {
      errorInput.value.title.isError = true
      errorInput.value.title.message = postArticle.error.message
    }
    return postArticle.error.statuscode
  }

  ResetInput()
  return true
}

function ValidateEmptyInput() {
  let valid = true
  for (const key in newArticle.value) {
    if (key !== 'additionalLink' && key !== 'descriptionHtml') {
      if (newArticle.value[key] === '') {
        errorInput.value[key].isError = true
        errorInput.value[key].message = 'Required'
        valid = false
      } else {
        errorInput.value[key].isError = false
        errorInput.value[key].message = ''
      }
    }
  }
  return valid
}

function ResetInput() {
  for (const key in newArticle.value) {
    newArticle.value[key] = ''
  }

  for (const key in errorInput.value) {
    errorInput.value[key].isError = false
    errorInput.value[key].message = ''
  }
}

export { categoryRef, newArticle, errorInput, GetCategoryReference, HandleSave, ResetInput }